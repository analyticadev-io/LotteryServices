using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using LotteryServices.Utilitys;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Services
{
    public class ServiceLogin : ILogin
    {
        private readonly LoteriaDbContext _context;
        private readonly Utilidades _utilidades;

        public ServiceLogin(LoteriaDbContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        public async Task<(bool IsSuccess, string Token)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == loginDto.NombreUsuario
                                              && u.Contrasena == _utilidades.EncriptarSHA256(loginDto.Contrasena));

                if (user == null)
                {
                    return (false, "");
                }

                // Assuming GenerarJwt method generates a JWT token for the user
                string token = _utilidades.GenerarJwt(user);

                return (true, token);
            }
            catch (Exception ex)
            {
                // Handle or log the exception appropriately
                throw new InvalidOperationException("An error occurred during login.", ex);
            }
        }


        public async Task RegistroAsync(UsuarioDto usuarioDto)
        {
            try
            {
                var existingUser = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == usuarioDto.NombreUsuario);
                if (existingUser)
                {
                    throw new InvalidOperationException("El Nombre de Usuario ya está en uso.");
                }

                var existingEmail = await _context.Usuarios
                    .AnyAsync(u => u.Email == usuarioDto.Email);
                if (existingEmail)
                {
                    throw new InvalidOperationException("El Email ya está en uso.");
                }

                var userModel = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Email = usuarioDto.Email,
                    Contrasena = _utilidades.EncriptarSHA256(usuarioDto.Contrasena),
                    NombreUsuario = usuarioDto.NombreUsuario,
                    FechaRegistro = usuarioDto.FechaRegistro,
                };

                await _context.Usuarios.AddAsync(userModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Manejar la excepción y proporcionar un mensaje adecuado
                throw new InvalidOperationException("Error al registrar el usuario. Asegúrese de que los datos sean únicos y válidos.", ex);
            }
        }



    }
}
