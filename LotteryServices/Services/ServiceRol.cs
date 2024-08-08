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




    }
}
