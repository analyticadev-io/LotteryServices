using LotteryServices.Dtos;
using LotteryServices.Models;

namespace LotteryServices.Interfaces
{
    public interface IPermiso
    {
        Task<IEnumerable<Permiso>> GetPermisosAsync();

        
    }
}
