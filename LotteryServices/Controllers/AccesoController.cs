
using Hangfire;
using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Utilitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LotteryServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccesoController : ControllerBase
    {
        private Utilidades _utilidades;
        private readonly ILogin _loginService;
        private readonly IEncriptado _encriptadoService;
        private readonly ISorteo _sorteoService;
        private readonly LoteriaDbContext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public AccesoController(Utilidades utilidades, ILogin loginService, IEncriptado encriptadoService, ISorteo sorteoService, LoteriaDbContext context, IBackgroundJobClient backgroundJobClient)
        {
            _utilidades = utilidades;
            _loginService = loginService;
            _encriptadoService = encriptadoService;
            _sorteoService = sorteoService;
            _context = context;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Email = usuarioDto.Email,
                    Contrasena = usuarioDto.Contrasena,
                    NombreUsuario = usuarioDto.NombreUsuario,
                    FechaRegistro = usuarioDto.FechaRegistro
                };

                // Llama al método asincrónico
                await _loginService.RegistroAsync(usuario);

                // Recupera el usuario recién registrado desde la base de datos
                var usuarioRegistrado = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == usuarioDto.NombreUsuario);

                if (usuarioRegistrado == null)
                {
                    return BadRequest(new { message = "No se pudo recuperar el usuario registrado." });
                }

                var usuarioRegistradoDto = new UsuarioDto
                {
                    UsuarioId = usuarioRegistrado.UsuarioId,
                    Nombre = usuarioRegistrado.Nombre,
                    Email = usuarioRegistrado.Email,
                    Contrasena = "",
                    NombreUsuario = usuarioRegistrado.NombreUsuario,
                    FechaRegistro = usuarioRegistrado.FechaRegistro
                };

                return CreatedAtAction(nameof(Registro), new { id = usuarioRegistradoDto.UsuarioId }, usuarioRegistradoDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(EncryptedRequest login)
        {
            //desifra lo datos recibidos
            var decryptedRequest = _encriptadoService.Decrypt(login.response);
            var objRequest = JsonConvert.DeserializeObject<Usuario>(decryptedRequest);

            var loginRequest = new LoginDto
            {
                NombreUsuario = objRequest.NombreUsuario,
                Contrasena = objRequest.Contrasena,
            };

            var (isSuccess, token) = await _loginService.LoginAsync(loginRequest);

            if (!isSuccess)
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid username or password." });
            }

            if (objRequest.NombreUsuario=="usuario1" && objRequest.Contrasena=="admin123.")
            {
                var userSpecificSorteo = await _sorteoService.AddAuxiliarySorteoAsync();
                BackgroundJob.Schedule(
                    () => _sorteoService.SaveAuxiliarySorteoWinnerAsync(userSpecificSorteo.SorteoId),
                    TimeSpan.FromMinutes(30)
                );

            }

            string serializedToken = JsonConvert.SerializeObject(token);
            var encryptedResponse = _encriptadoService.Encrypt(serializedToken);

            // Aquí puedes cifrar el token si es necesario
            return Ok(new { isSuccess = true, encryptedResponse });
        }


    }
}
