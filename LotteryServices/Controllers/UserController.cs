using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LotteryServices.Interfaces;
using LotteryServices.Services;
using LotteryServices.Models;

namespace LotteryServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUsuario _usuarioService;

        public UserController(IUsuario usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/loteria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuariosAsync();
            return Ok(usuarios);
        }

        // GET: api/loteria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: api/loteria
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            var newUsuario = await _usuarioService.AddUsuarioAsync(usuario);
            return newUsuario;
        }
    }
}
