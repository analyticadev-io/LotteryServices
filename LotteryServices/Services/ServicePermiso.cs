using BasicBackendTemplate.Models;
using LotteryServices.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Services
{
    public class ServicePermiso : IPermiso
    {
        private readonly BasicBackendTemplateContext _context;

        public ServicePermiso(BasicBackendTemplateContext context)
        {
            _context = context;
        }

   
        async Task<IEnumerable<Permiso>> IPermiso.GetPermisosAsync()
        {
            return await _context.Permisos.ToListAsync();
        }
    }
}
