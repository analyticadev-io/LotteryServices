using LotteryServices.Dtos;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Interfaces
{
    public interface ILogin
    {
        public Task RegistroAsync(Usuario usuario);
        Task<(bool IsSuccess, AuthResponseDto AuthResponse)> LoginAsync(LoginDto loginDto);     

    }
}
