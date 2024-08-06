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

    }
}
