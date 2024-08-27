using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class SorteosController : Controller
    {
        private readonly ISorteo _serviceSorteo;
        private readonly IEncriptado _serviceEncriptado;

        public SorteosController(ISorteo serviceSorteo, IEncriptado serviceEncriptado)
        {
            _serviceSorteo = serviceSorteo;
            _serviceEncriptado = serviceEncriptado;
        }



        //GET : /api/Sorteos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sorteo>>> GetSorteos()
        {
            var sorteos = await _serviceSorteo.GetAllStatusSorteosAsync();

            var jsonSorteos = JsonConvert.SerializeObject(sorteos);
            var encryptRequest = _serviceEncriptado.Encrypt(jsonSorteos);
            var req = new EncryptedResponse
            {
                response = encryptRequest,
            };

            return Ok(req);
        }

        //GET : /api/Sorteos
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Sorteo>>> EditSorteo(EncryptedRequest sorteo)
        {
            var decryptedResponse = _serviceEncriptado.Decrypt(sorteo.response);
            var sorteoObj = JsonConvert.DeserializeObject<Sorteo>(decryptedResponse);

            var editedSorteo = await _serviceSorteo.EditSorteoAsync(sorteoObj);

            var jsonSorteos = JsonConvert.SerializeObject(editedSorteo);
            var encryptRequest = _serviceEncriptado.Encrypt(jsonSorteos);
            var req = new EncryptedResponse
            {
                response = encryptRequest,
            };

            return Ok(req);
            
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Sorteo>>> AddSorteo(EncryptedRequest sorteo)
        {
            var decryptedResponse = _serviceEncriptado.Decrypt(sorteo.response);
            var sorteoObj = JsonConvert.DeserializeObject<Sorteo>(decryptedResponse);

            var sorteoAdd = await _serviceSorteo.AddSorteoAsync(sorteoObj);

            var jsonSorteos = JsonConvert.SerializeObject(sorteoAdd);
            var encryptRequest = _serviceEncriptado.Encrypt(jsonSorteos);
            var req = new EncryptedResponse
            {
                response = encryptRequest,
            };

            return Ok(req);

        }

        [HttpDelete("{encryptedId}")]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteSorteo(string  encryptedId)
        {

            var decryptedId = _serviceEncriptado.Decrypt(encryptedId);


            var sorteos = await _serviceSorteo.DeleteSorteoAsync(Convert.ToInt32(decryptedId));
            if (sorteos)
            {
                var jsonSorteos = JsonConvert.SerializeObject(sorteos);
                var encryptRequest = _serviceEncriptado.Encrypt(jsonSorteos);
                var req = new EncryptedResponse
                {
                    response = encryptRequest,
                };

                return Ok(req);
            }
            else
            {
                return NotFound();
            }
            
        }


    }
}
