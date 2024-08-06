using LotteryServices.Dtos;

namespace LotteryServices.Interfaces
{
    public interface ILogin
    {
        public Task RegistroAsync(UsuarioDto usuarioDto);
        public Task<(bool IsSuccess, string Token)> LoginAsync(LoginDto loginDto);
    }
}
