using BasicBackendTemplate.Dtos;
using BasicBackendTemplate.Models;

namespace BasicBackendTemplate.Interfaces
{
    public interface IModule
    {
        Task<Module> AddModuleAsync(ResponseModule module);
        Task<Module> EditModuleAsync(ResponseModule module);
        Task <IEnumerable<Module>> GetModulesAsync();

        Task<bool> DeleteModuleAsync(int module);

    }
}
