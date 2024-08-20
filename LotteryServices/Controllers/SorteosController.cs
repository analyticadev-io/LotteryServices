using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class SorteosController : Controller
    {
        private readonly ISorteo _serviceSorteo;

        public SorteosController(ISorteo serviceSorteo)
        {
            _serviceSorteo = serviceSorteo;
        }

        //GET : /api/Sorteos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sorteo>>> GetSorteos()
        {
            var sorteos = await _serviceSorteo.GetSorteosAsync();
            return Ok(sorteos);
        }

        //GET : /api/Sorteos
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Sorteo>>> EditSorteo(Sorteo sorteo)
        {
            var sorteos = await _serviceSorteo.EditSorteoAsync(sorteo);
            return Ok(sorteos);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Sorteo>>> AddSorteo(Sorteo sorteo)
        {
            var sorteos = await _serviceSorteo.AddSorteoAsync(sorteo);
            return Ok(sorteos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteSorteo(int id)
        {
            var sorteos = await _serviceSorteo.DeleteSorteoAsync(id);
            if (sorteos)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }


    }
}
