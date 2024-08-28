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
            var newSorteo = new Sorteo();
            newSorteo.Title = sorteo.Title;
            newSorteo.Descripcion = sorteo.Descripcion;
            newSorteo.FechaSorteo = sorteo.FechaSorteo;
            newSorteo.Status = "active";
            _context.Sorteos.Add(newSorteo);
            await _context.SaveChangesAsync();
            return newSorteo;
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
                existingSorteo.Title = sorteo.Title;
                existingSorteo.FechaSorteo = sorteo.FechaSorteo;
                existingSorteo.Descripcion = sorteo.Descripcion;
                existingSorteo.Status = "active";

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

        public async Task<IEnumerable> GetSorteosActiveAsync()
        {
            return await _context.Sorteos.Where(s => s.Status == "active").ToListAsync();
        }

        public async Task<IEnumerable> GetSorteosCompleteAsync()
        {
            return await _context.Sorteos.Where(s => s.Status == "active").ToListAsync();
        }

        public async Task<IEnumerable> GetAllStatusSorteosAsync()
        {
            return await _context.Sorteos.Include(s=>s.NumerosSorteos).ToListAsync();
        }

        public async Task<Sorteo> SaveSorteoWinnerAsync(Sorteo sorteo)
        {
            // Busca el sorteo existente por su ID
            var existingSorteo = await _context.Sorteos
                .Include(s => s.NumerosSorteos) // Asegúrate de incluir NumerosSorteos si es necesario
                .FirstOrDefaultAsync(s => s.SorteoId == sorteo.SorteoId);

            if (existingSorteo != null)
            {
                // Actualiza el estado y los números del sorteo
                existingSorteo.Status = "complete";
                existingSorteo.NumerosSorteos = sorteo.NumerosSorteos;

                // Marca el objeto como modificado y guarda los cambios
                _context.Sorteos.Update(existingSorteo);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    // Manejo de excepciones específico para errores de actualización
                    throw new Exception("Error al guardar el número ganador del sorteo", ex);
                }

                return existingSorteo;
            }
            else
            {
                // Manejo del caso donde el sorteo no se encuentra
                throw new KeyNotFoundException("Sorteo no encontrado con el ID proporcionado.");
            }
        }


    }
}
