using LotteryServices.Dtos;
using LotteryServices.Models;

namespace LotteryServices.Interfaces
{
    public interface IRol
    {
        Task<IEnumerable<Rol>> GetRolesAsync();
        Task<Rol> GetRolByIdAsync(int id);
        Task<Rol> AddRolWithPermissionsAsync(Rol rol);
        Task<Rol> UpdateWithPermissionsRolsync(Rol rol);
        Task<bool> DeleteRolAsync(int rolId);
        Task<bool> AsignarRolAUsuarioAsync(int usuarioId, int rolId);
        Task<bool> EditarRolAUsuarioAsync(int usuarioId, int oldRolId, int newRolId);
        Task<bool> EliminarRolAUsuarioAsync(int usuarioId,  int RolId);
    }
}
