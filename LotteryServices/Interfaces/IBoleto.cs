using LotteryServices.Dtos;
using LotteryServices.Models;

namespace LotteryServices.Interfaces
{
    public interface IBoleto
    {
        Task<Boleto> AddBoletoAsync(Boleto boleto);
        Task<Boleto> EditBoletoAsync(Boleto boleto);
        Task<IEnumerable<Boleto>> GetBoletosAsync();

        Task<bool> DeleteBoletoAsync(int boleto);
        Task<Boleto> GetBoletoByNumAsync(int boleto);
        Task<ICollection<Boleto>> GetAllBoletoByUserAsync(int userId);
        Task<ICollection<Boleto>> GetBoletoActiveByUserAsync(int userId);
        Task<ICollection<Boleto>> GetBoletoCompleteByUserAsync(int userId);
    }
}
