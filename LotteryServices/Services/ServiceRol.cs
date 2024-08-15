using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LotteryServices.Services
{
    public class ServiceRol : IRol
    {
        private readonly LoteriaDbContext _context;

        public ServiceRol(LoteriaDbContext context)
        {
            _context = context;
        }

        public async Task<Rol> AddRolWithPermissionsAsync(Rol rol)
        {
            try
            {
                var newRol = new Rol
                {
                    Nombre = rol.Nombre
                };

                // Obtener los permisos existentes y agregarlos al nuevo rol
                foreach (var permisoId in rol.Permisos.Select(p => p.PermisoId))
                {
                    var permiso = await _context.Permisos.FindAsync(permisoId);
                    if (permiso != null)
                    {
                        newRol.Permisos.Add(permiso);
                    }
                }

                _context.Rols.Add(newRol);
                await _context.SaveChangesAsync();
                return newRol;
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
                var rolToDelete = await _context.Rols
                   .Include(r => r.Permisos)
                   .FirstOrDefaultAsync(r => r.RolId == rolId);

                if (rolToDelete == null)
                {
                    throw new Exception("Rol no encontrado");
                }

                rolToDelete.Permisos.Clear();
                _context.Rols.Remove(rolToDelete);
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (Exception)
            {
                // Puedes registrar el error aquí, por ejemplo con un logger
                throw; // Lanza la excepción para que pueda ser manejada por el llamador
            }
        }


        public async Task<Rol> GetRolByIdAsync(int id)
        {
            try
            {
                var rol = await _context.Rols
                    .Include(r => r.Permisos)
                    .FirstOrDefaultAsync(r => r.RolId == id);

                if (rol == null)
                {
                    throw new Exception($"Rol con ID {id} no encontrado");
                }

                return rol;
            }
            catch (DbException ex)
            {
                // Manejar excepciones específicas de la base de datos
                throw new Exception("Error al obtener el rol", ex);
            }
        }


        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            try
            {
                 return await _context.Rols.Include(r=>r.Permisos).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Rol> UpdateWithPermissionsRolsync(Rol rol)
        {

            try
            {
                var rolToUpdate = await _context.Rols
                    .Include(r => r.Permisos)
                    .FirstOrDefaultAsync(r => r.RolId == rol.RolId);

                if (rolToUpdate == null)
                {
                    throw new Exception("Rol no encontrado");
                }

                rolToUpdate.Permisos.Clear();
                foreach (var permisoId in rol.Permisos.Select(p => p.PermisoId))
                {
                    var permiso = await _context.Permisos.FindAsync(permisoId);
                    if (permiso != null)
                    {
                        rolToUpdate.Permisos.Add(permiso);
                    }
                }

                _context.Rols.Update(rolToUpdate);
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
