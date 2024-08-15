using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Services
{
    public class ServicePermiso : IPermiso
    {
        private readonly LoteriaDbContext _context;

        public ServicePermiso(LoteriaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permiso>> GetPermisosAsync()
        {
           return await _context.Permisos.ToListAsync();
        }
    }
}
