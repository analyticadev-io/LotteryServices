using BasicBackendTemplate.Models;
using LotteryServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IPermiso _permisoService;

        public PermisosController(IPermiso permisoService)
        {
            _permisoService = permisoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisos()
        {
            var usuarios = await _permisoService.GetPermisosAsync();
            return Ok(usuarios);
        }
    }
}
