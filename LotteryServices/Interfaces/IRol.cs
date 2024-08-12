using LotteryServices.Models;

namespace LotteryServices.Interfaces
{
    public interface IRol
    {
        Task<IEnumerable<Rol>> GetRolesAsync();
        Task<IEnumerable<Rol>> GetRolAsync(int id);
        Task<Rol> AddRolAsync(Rol rol);
        Task<Rol> UpdateRolsync(Rol rol);
        Task<bool> DeleteRolAsync(int id);
        Task<bool> AsignarRolAUsuarioAsync(int usuarioId, int rolId);
        Task<bool> EditarRolAUsuarioAsync(int usuarioId, int oldRolId, int newRolId);
        Task<bool> EliminarRolAUsuarioAsync(int usuarioId,  int RolId);
    }
}
