using BasicBackendTemplate.Models;
using LotteryServices.Interfaces;

using Microsoft.EntityFrameworkCore;
using static LotteryServices.Services.ServiceUsuario;

namespace LotteryServices.Services
{
    public class ServiceUsuario : IUsuario
    {


        private readonly BasicBackendTemplateContext _context;

        public ServiceUsuario(BasicBackendTemplateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rols) 
                .ToListAsync();       
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> AddUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }       
}
