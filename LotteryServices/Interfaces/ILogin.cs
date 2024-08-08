using LotteryServices.Models;
using LotteryServices.Dtos;

namespace LotteryServices.Interfaces
{
    public interface ILogin
    {
        public Task RegistroAsync(Usuario usuario);
        public Task<(bool IsSuccess, string Token)> LoginAsync(LoginDto loginDto);
    }
}
