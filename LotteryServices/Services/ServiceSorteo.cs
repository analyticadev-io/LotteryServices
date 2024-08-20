using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Globalization;

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

        public async Task<bool> DeleteSorteoAsync(int id)
        {
            var existingSorteo = await _context.Sorteos.FirstOrDefaultAsync(s => s.SorteoId == id);

            if (existingSorteo != null)
            {
                _context.Sorteos.Remove(existingSorteo);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {

                return false;
            }
        }

        public async Task<Sorteo> EditSorteoAsync(Sorteo sorteo)
        {
            var existingSorteo = await _context.Sorteos.FirstOrDefaultAsync(s => s.SorteoId == sorteo.SorteoId);

            if (existingSorteo != null)
            {
                existingSorteo.FechaSorteo = sorteo.FechaSorteo;
                existingSorteo.Descripcion = sorteo.Descripcion;
                existingSorteo.Status = sorteo.Status;

                _context.Sorteos.Update(existingSorteo);
                await _context.SaveChangesAsync();

                return existingSorteo;
            }
            else
            {
                // Manejar el caso cuando no se encuentra el sorteo
                throw new Exception("Sorteo no encontrado.");
            }
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
