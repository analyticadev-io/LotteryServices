using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotteryServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRol _serviceRol;
        private readonly IEncriptado _encriptService;

        public RolesController(IRol serviceRol, IEncriptado encriptService)
        {
            _serviceRol = serviceRol;
            _encriptService = encriptService;
        }




        //GET : /api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            var roles = await _serviceRol.GetRolesAsync();
            
            var jsonResponse = JsonConvert.SerializeObject(roles);
            var encryptedResponse = new EncryptedResponse
            {
                response = _encriptService.Encrypt(jsonResponse)
            };

            return Ok(encryptedResponse);

        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Rol>>> addRol(EncryptedRequest crypt)
        {
            try
            {
                // Decrypt the encrypted role data
                var decryptedRol = _encriptService.Decrypt(crypt.response);

                // Deserialize the decrypted data to a Rol object
                var rol = JsonConvert.DeserializeObject<Rol>(decryptedRol);
                if (rol == null)
                {
                    return BadRequest("Invalid role data.");
                }

                // Add the role with its associated permissions
                var nuevorol = await _serviceRol.AddRolWithPermissionsAsync(rol);
                if (nuevorol == null)
                {
                    return StatusCode(500, "Failed to add role.");
                }

                // Serialize the newly created role back to JSON
                string serializedRol = JsonConvert.SerializeObject(nuevorol);

                // Encrypt the serialized JSON          
                return Ok(new EncryptedResponse { response = _encriptService.Encrypt(serializedRol) });
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors
                return BadRequest($"Invalid JSON data: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Handle decryption errors
                return BadRequest($"Decryption error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut]
        public async Task<ActionResult<Rol>> UpdateRol(EncryptedRequest crypt)
        {
            try
            {
                // Decrypt the encrypted role data
                var decryptedRol = _encriptService.Decrypt(crypt.response);

                // Deserialize the decrypted data to a Rol object
                var rol = JsonConvert.DeserializeObject<Rol>(decryptedRol);
                if (rol == null)
                {
                    return BadRequest("Invalid role data.");
                }

                var updatedRol = await _serviceRol.UpdateWithPermissionsRolsync(rol);
                if (updatedRol == null)
                {
                    return NotFound("Rol no encontrado.");
                }

                // Serialize the newly created role back to JSON
                string serializedRol = JsonConvert.SerializeObject(updatedRol);

                return Ok(new EncryptedResponse { response = _encriptService.Encrypt(serializedRol) });
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                return StatusCode(500, "Error al actualizar el rol.");
            }
        }

        [HttpDelete("{rolId}")]
        public async Task<ActionResult> DeleteRol(string rolId)
        {
            try
            {
                var decryptedId = _encriptService.Decrypt(rolId);

                var result = await _serviceRol.DeleteRolAsync(Convert.ToInt32(decryptedId));
                if (!result)
                {
                    return NotFound("Rol no encontrado.");
                }

                return Ok(new EncryptedResponse { response="OK"}); // 204 No Content, ya que la eliminación fue exitosa
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