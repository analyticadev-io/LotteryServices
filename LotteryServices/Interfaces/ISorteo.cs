using LotteryServices.Models;
using System.Collections;

namespace LotteryServices.Interfaces
{
    public interface ISorteo
    {
        Task<Sorteo> AddSorteoAsync(Sorteo sorteo);
        Task<IEnumerable> GetSorteosAsync();
        Task<Sorteo> GetSorteoByIdAsync(int id);
    }
}
