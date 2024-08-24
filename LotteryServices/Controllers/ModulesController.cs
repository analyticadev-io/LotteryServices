using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModule _moduleService;
        private readonly IEncriptado _encryptService;

        private readonly LoteriaDbContext _context;

        public ModulesController(IModule moduleService, LoteriaDbContext context, IEncriptado encryptService)
        {
            _moduleService = moduleService;
            _context = context;
            _encryptService = encryptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            var modules = await _moduleService.GetModulesAsync();
            var jsonModules = JsonConvert.SerializeObject(modules);
            var cryptedResponse = _encryptService.Encrypt(jsonModules);
            var crypt = new EncryptedResponse
            {
                response = cryptedResponse,
            };

            return Ok(crypt);
        }

        [HttpPost]
        public async Task<ActionResult<Module>> AddModule(EncryptedRequest module)
        {
            var decryptedRequest = _encryptService.Decrypt(module.response);
            var objResponse = JsonConvert.DeserializeObject<ResponseModule>(decryptedRequest);
            
            var newModule = await _moduleService.AddModuleAsync(objResponse);

            var JsonResponse = JsonConvert.SerializeObject(newModule);
            var encryptResponse = _encryptService.Encrypt(JsonResponse);

            var crypt = new EncryptedResponse
            {
                response = encryptResponse,
            };

            return Ok(crypt);
        }

        [HttpPut]
        public async Task<ActionResult<Module>> EditModule(EncryptedRequest module)
        {

            var decryptedRequest = _encryptService.Decrypt(module.response);
            var objResponse = JsonConvert.DeserializeObject<ResponseModule>(decryptedRequest);

            var newModule = await _moduleService.EditModuleAsync(objResponse);
            
            var JsonResponse = JsonConvert.SerializeObject(newModule);
            var encryptResponse = _encryptService.Encrypt(JsonResponse);

            var crypt = new EncryptedResponse
            {
                response = encryptResponse,
            };

            return Ok(crypt);
        }

        [HttpDelete("{module}")]
        public async Task<ActionResult> DeleteModule(string module)
        {
            var decryptedRequest = _encryptService.Decrypt(module);
            
           

            bool isDeleted = await _moduleService.DeleteModuleAsync(Convert.ToInt32(decryptedRequest));

            if (isDeleted)
            {
                return Ok(new EncryptedResponse { response=_encryptService.Encrypt("OK")});
            }
            else
            {
                return BadRequest(new { message = "Hubo un problema al eliminar el módulo." });
            }




        }


    }
}
