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
    public class BoletosController : ControllerBase
    {
        private readonly IBoleto _boletoService;
        private readonly IEncriptado _encryptService;

        private readonly LoteriaDbContext _context;

        public BoletosController(IBoleto boletoService, IEncriptado encryptService, LoteriaDbContext context)
        {
            _boletoService = boletoService;
            _encryptService = encryptService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Boleto>> AddBoleto(EncryptedRequest module)
        {
            var decryptedRequest = _encryptService.Decrypt(module.response);
            var objResponse = JsonConvert.DeserializeObject<Boleto>(decryptedRequest);

            var newModule = await _boletoService.AddBoletoAsync(objResponse);

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var JsonResponse = JsonConvert.SerializeObject(newModule, jsonSettings);
            var encryptResponse = _encryptService.Encrypt(JsonResponse);

            var crypt = new EncryptedResponse
            {
                response = encryptResponse,
            };

            return Ok(crypt);
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<Boleto>> GetUserBoleto(string idUser)
        {
            var decryptedRequest = _encryptService.Decrypt(idUser);
            var userBoletos = await _boletoService.GetBoletoActiveByUserAsync(Convert.ToInt32(decryptedRequest));

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var JsonResponse = JsonConvert.SerializeObject(userBoletos, jsonSettings);
            var encryptResponse = _encryptService.Encrypt(JsonResponse);

            var crypt = new EncryptedResponse
            {
                response = encryptResponse,
            };

            return Ok(crypt);

        }

    }
}
