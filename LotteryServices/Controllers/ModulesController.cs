using BasicBackendTemplate.Dtos;
using BasicBackendTemplate.Interfaces;
using BasicBackendTemplate.Models;
using LotteryServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicBackendTemplate.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModule _moduleService;

        private readonly BasicBackendTemplateContext _context;

        public ModulesController(IModule moduleService, BasicBackendTemplateContext context)
        {
            _moduleService = moduleService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            var modules = await _moduleService.GetModulesAsync();
            return Ok(modules);
        }

        [HttpPost]
        public async Task<ActionResult<Module>> AddModule(ResponseModule module)
        {
            var newModule = await _moduleService.AddModuleAsync(module);
            return Ok(newModule);
        }

        [HttpPut]
        public async Task<ActionResult<Module>> EditModule(ResponseModule module)
        {
            var newModule = await _moduleService.EditModuleAsync(module);
            return Ok(newModule);
        }

        [HttpDelete("{module}")]
        public async Task<ActionResult> DeleteModule(int module)
        {
            try
            {
                bool isDeleted = await _moduleService.DeleteModuleAsync(module);

                if (isDeleted)
                {
                    return Ok(new { message = "El módulo y sus permisos han sido eliminados con éxito." });
                }
                else
                {
                    return BadRequest(new { message = "Hubo un problema al eliminar el módulo." });
                }
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud.", details = ex.Message });
            }
        }


    }
}
