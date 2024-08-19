
using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Utilitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccesoController : ControllerBase
    {
        private Utilidades _utilidades;
        private readonly ILogin _loginService;
        private readonly LoteriaDbContext _context;

        public AccesoController(Utilidades utilidades, ILogin loginService, LoteriaDbContext context)
        {
            _utilidades = utilidades;
            _loginService = loginService;
            _context = context;
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
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var (isSuccess, token) = await _loginService.LoginAsync(loginDto);

            if (!isSuccess)
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid username or password." });
            }

            return Ok(new { isSuccess = true, token });
        }


    }
}
