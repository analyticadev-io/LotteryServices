using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Services;
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
            var nuevorol = await _serviceRol.AddRolWithPermissionsAsync(rol);
            return Ok(nuevorol);

        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<Rol>>> GetRol(int id)
        //{
        //    var rol = await _serviceRol.GetRolByIdAsync(id);
        //    return Ok(rol);

        //}

        [HttpPut]
        public async Task<ActionResult<Rol>> UpdateRol( Rol rol)
        {
            try
            {
                var updatedRol = await _serviceRol.UpdateWithPermissionsRolsync(rol);
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

        [HttpDelete("{rolId}")]
        public async Task<ActionResult> DeleteRol(int rolId)
        {
            try
            {
                var result = await _serviceRol.DeleteRolAsync(rolId);
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


        [HttpPost("asignar-rol")]
        public async Task<IActionResult> AsignarRolAUsuario([FromBody] UserRolRequest request)
        {
            try
            {
                bool resultado = await _serviceRol.AsignarRolAUsuarioAsync(request.UsuarioId, request.RolId);

                if (resultado)
                {
                    return Ok("Rol asignado exitosamente.");
                }
                else
                {
                    return BadRequest("No se pudo asignar el rol.");
                }
            }
            catch (Exception ex)
            {
                // Registra el error aquí, por ejemplo con un logger
                return StatusCode(500, $"Error al asignar el rol: {ex.Message}");
            }
        }


        [HttpPut("editar-rol")]
        public async Task<IActionResult> EditarRolAUsuario([FromBody] UserRolRequest request)
        {
            try
            {
                bool resultado = await _serviceRol.EditarRolAUsuarioAsync(request.UsuarioId, request.RolIdAEliminar, request.RolId);

                if (resultado)
                {
                    return Ok("Rol actualizado exitosamente.");
                }
                else
                {
                    return BadRequest("No se pudo actualizar el rol.");
                }
            }
            catch (Exception ex)
            {
                // Registra el error aquí, por ejemplo con un logger
                return StatusCode(500, $"Error al actualizar el rol: {ex.Message}");
            }
        }


        [HttpPost("eliminar-rol")]
        public async Task<IActionResult> EliminarRolDeUsuario([FromBody] UserRolRequest request)
        {
            try
            {
                bool resultado = await _serviceRol.EliminarRolAUsuarioAsync(request.UsuarioId, request.RolId);

                if (resultado)
                {
                    return Ok("Rol eliminado exitosamente.");
                }
                else
                {
                    return BadRequest("No se pudo eliminar el rol.");
                }
            }
            catch (Exception ex)
            {
                // Registra el error aquí, por ejemplo con un logger
                return StatusCode(500, $"Error al eliminar el rol: {ex.Message}");
            }
        }


    }
}