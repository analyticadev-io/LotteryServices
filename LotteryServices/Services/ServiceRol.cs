using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Services
{
    public class ServiceRol : IRol
    {
        private readonly LoteriaDbContext _context;

        public ServiceRol(LoteriaDbContext context)
        {
            _context = context;
        }

        public async Task<Rol> AddRolAsync(Rol rol)
        {
            try
            {
                _context.Rols.Add(rol);
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteRolAsync(int rolId)
        {
            try
            {
                // Encuentra el rol en el contexto
                var rol = await _context.Rols.FindAsync(rolId);
                if (rol == null)
                {
                    return false; // Rol no encontrado
                }

                _context.Rols.Remove(rol);
                await _context.SaveChangesAsync();
                return true; // Rol eliminado exitosamente
            }
            catch (Exception)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                throw; // Lanza la excepción para que pueda ser manejada por el llamador
            }
        }


        public async Task<IEnumerable<Rol>> GetRolAsync(int id)
        {
            try
            {
                // Encuentra el rol con el id dado
                var rol = await _context.Rols
                    .Where(r => r.RolId == id)
                    .ToListAsync();

                return rol;
            }
            catch (Exception ex)
            {
    
                return Enumerable.Empty<Rol>(); 
            }
        }


        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            try
            {
                var roles = await _context.Rols.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Rol> UpdateRolsync(Rol rol)
        {
            try
            {
                // Encuentra el rol existente en el contexto
                var rolToUpdate = await _context.Rols.FindAsync(rol.RolId);

                if (rolToUpdate == null)
                {
                    
                    return null;
                }

                // Actualiza las propiedades del rol
                _context.Entry(rolToUpdate).CurrentValues.SetValues(rol);
                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();
                return rolToUpdate;

            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }


        public async Task<bool> AsignarRolAUsuarioAsync(int usuarioId, int rolId)
        {
            try
            {
                // Verificar si el usuario existe
                var usuario = await _context.Usuarios
                    .Include(u => u.Rols)
                    .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                // Verificar si el rol existe
                var rol = await _context.Rols.FindAsync(rolId);
                if (rol == null)
                {
                    throw new Exception("Rol no encontrado");
                }

                // Verificar si el usuario ya tiene el rol asignado
                if (usuario.Rols.Any(r => r.RolId == rolId))
                {
                    throw new Exception("El usuario ya tiene este rol asignado");
                }

                // Asignar el rol al usuario
                usuario.Rols.Add(rol);
                await _context.SaveChangesAsync();

                return true; // Rol asignado exitosamente
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                throw ex;
            }
        }

        public async Task<bool> EditarRolAUsuarioAsync(int usuarioId,int oldRolId, int newRolId)
        {

            try
            {
                // Obtener el usuario con los roles actuales
                var usuario = await _context.Usuarios
                    .Include(u => u.Rols)
                    .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                // Verificar si el rol a eliminar existe en los roles del usuario
                var rolExistente = usuario.Rols.FirstOrDefault(r => r.RolId == oldRolId);
                if (rolExistente != null)
                {
                    // Eliminar el rol viejo
                    usuario.Rols.Remove(rolExistente);
                }

                // Verificar si el nuevo rol existe
                var nuevoRol = await _context.Rols.FindAsync(newRolId);
                if (nuevoRol == null)
                {
                    throw new Exception("Nuevo rol no encontrado");
                }

                // Verificar si el nuevo rol ya está asignado
                if (!usuario.Rols.Any(r => r.RolId == newRolId))
                {
                    // Asignar el nuevo rol al usuario
                    usuario.Rols.Add(nuevoRol);
                }

                // Guardar los cambios
                await _context.SaveChangesAsync();

                return true; // Rol actualizado exitosamente
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                throw ex;
            }

        }

        public async Task<bool> EliminarRolAUsuarioAsync(int usuarioId, int rolId)
        {

            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rols)
                    .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                var rol = usuario.Rols.FirstOrDefault(r => r.RolId == rolId);

                if (rol == null)
                {
                    throw new Exception("Rol no encontrado en el usuario");
                }

                usuario.Rols.Remove(rol);
                await _context.SaveChangesAsync();

                return true; // Rol eliminado exitosamente
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                throw ex;
            }


        }
    }
}
