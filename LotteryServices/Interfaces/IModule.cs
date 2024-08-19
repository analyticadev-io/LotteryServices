using LotteryServices.Dtos;
using LotteryServices.Models;

namespace LotteryServices.Interfaces
{
    public interface IModule
    {
        Task<Module> AddModuleAsync(ResponseModule module);
        Task<Module> EditModuleAsync(ResponseModule module);
        Task<IEnumerable<Module>> GetModulesAsync();

        Task<bool> DeleteModuleAsync(int module);

    }
}
