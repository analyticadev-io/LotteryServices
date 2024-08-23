using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IPermiso _permisoService;
        private readonly IEncriptado _encriptService;

        public PermisosController(IPermiso permisoService, IEncriptado encriptadoService)
        {
            _permisoService = permisoService;
            _encriptService = encriptadoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisos()
        {
            var permisos = await _permisoService.GetPermisosAsync();

            var jsonResponse = JsonConvert.SerializeObject(permisos);
            var encryptedResponse = new EncryptedResponse
            {
                response = _encriptService.Encrypt(jsonResponse)
            };

            return Ok(encryptedResponse);
        }
    }
}
