using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace LotteryServices.Services
{
    public class ServiceSorteo : ISorteo
    {
        private readonly LoteriaDbContext _context;

        public ServiceSorteo(LoteriaDbContext context)
        {
            _context = context;
        }

        public async Task<Sorteo> AddSorteoAsync(Sorteo sorteo)
        {
            _context.Sorteos.Add(sorteo);
            await _context.SaveChangesAsync();
            return sorteo;
        }

        public async Task<Sorteo> GetSorteoByIdAsync(int id)
        {
            return await _context.Sorteos.FindAsync(id);
        }

        public async Task<IEnumerable> GetSorteosAsync()
        {
            return await _context.Sorteos.ToListAsync();
        }
    }
}
