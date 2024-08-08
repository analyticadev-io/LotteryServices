using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRol _serviceRol;

        public RolesController(IRol servicerol)
        {
            _serviceRol = servicerol;
        }

        //GET : /api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            var roles = await _serviceRol.GetRolesAsync();
            return Ok(roles);

        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Rol>>> addRol(Rol rol)
        {
            var nuevorol = await _serviceRol.AddRolAsync(rol);
            return Ok(nuevorol);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRol(int id)
        {
            var rol = await _serviceRol.GetRolAsync(id);
            return Ok(rol);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Rol>> UpdateRol(int id, Rol rol)
        {
            if (id != rol.RolId)
            {
                return BadRequest("El ID del rol no coincide.");
            }

            try
            {
                var updatedRol = await _serviceRol.UpdateRolsync(rol);
                if (updatedRol == null)
                {
                    return NotFound("Rol no encontrado.");
                }

                return Ok(updatedRol);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                return StatusCode(500, "Error al actualizar el rol.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRol(int id)
        {
            try
            {
                var result = await _serviceRol.DeleteRolAsync(id);
                if (!result)
                {
                    return NotFound("Rol no encontrado.");
                }

                return NoContent(); // 204 No Content, ya que la eliminación fue exitosa
            }
            catch (Exception)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                return StatusCode(500, "Error al eliminar el rol.");
            }
        }

    }
}