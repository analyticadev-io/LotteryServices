using LotteryServices.Models;
using System.Collections;

namespace LotteryServices.Interfaces
{
    public interface ISorteo
    {
        Task<Sorteo> AddSorteoAsync(Sorteo sorteo);
        Task<Sorteo> EditSorteoAsync(Sorteo sorteo);
        Task<bool> DeleteSorteoAsync(int id);
        Task<IEnumerable> GetSorteosAsync();
        Task<Sorteo> GetSorteoByIdAsync(int id);
    }
}
