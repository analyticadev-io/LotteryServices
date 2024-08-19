using LotteryServices.Models;

namespace LotteryServices.Interfaces
{
    public interface IUsuario
    {
        Task<IEnumerable<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task<Usuario> AddUsuarioAsync(Usuario usuario);


    }
}
