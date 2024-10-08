﻿using LotteryServices.Models;
using System.Collections;

namespace LotteryServices.Interfaces
{
    public interface ISorteo
    {
        Task<Sorteo> AddSorteoAsync(Sorteo sorteo);
        Task<Sorteo> EditSorteoAsync(Sorteo sorteo);
        Task<bool> DeleteSorteoAsync(int id);
        Task<IEnumerable> GetSorteosActiveAsync();
        Task<IEnumerable> GetSorteosCompleteAsync();
        Task<IEnumerable> GetAllStatusSorteosAsync();
        Task<Sorteo> GetSorteoByIdAsync(int id);
        Task<Sorteo> SaveSorteoWinnerAsync(Sorteo sorteo);
        Task<Sorteo> SaveAuxiliarySorteoWinnerAsync(int sorteo);

        Task<Sorteo> AddAuxiliarySorteoAsync();

        public int generateWinner();

    }
}
