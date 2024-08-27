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
        public async Task<ActionResult<EncryptedResponse>> AddBoleto(EncryptedRequest module)
        {
            try
            {
                // Desencriptar y deserializar la solicitud
                var decryptedRequest = _encryptService.Decrypt(module.response);
                var boleto = JsonConvert.DeserializeObject<Boleto>(decryptedRequest);

                // Intentar agregar el boleto
                var newBoleto = await _boletoService.AddBoletoAsync(boleto);

                // Serializar y encriptar la respuesta
                var jsonSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var jsonResponse = JsonConvert.SerializeObject(newBoleto, jsonSettings);
                var encryptResponse = _encryptService.Encrypt(jsonResponse);

                return Ok(new EncryptedResponse { response = encryptResponse });
            }
            catch (InvalidOperationException ex)
            {
                // Manejar casos específicos como "Ya existe un boleto con esos números en el sorteo"
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejar errores generales
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ocurrió un error al procesar la solicitud." });
            }
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
