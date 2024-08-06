using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Utilitys;
using Microsoft.AspNetCore.Mvc;

namespace LotteryServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccesoController : ControllerBase
    {
        private Utilidades _utilidades;
        private readonly ILogin _loginService;

        public AccesoController(Utilidades utilidades, ILogin loginService)
        {
            _utilidades = utilidades;
            _loginService = loginService;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(UsuarioDto usuarioDto)
        {
            await _loginService.RegistroAsync(usuarioDto);
            return CreatedAtAction("Registro", new { id = usuarioDto.NombreUsuario }, usuarioDto);
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
