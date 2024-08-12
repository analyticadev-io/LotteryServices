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

        //public async Task<(bool IsSuccess, string Token)> LoginAsync(LoginDto loginDto)
        //{
        //    try
        //    {
        //        var user = await _context.Usuarios
        //            .FirstOrDefaultAsync(u => u.NombreUsuario == loginDto.NombreUsuario
        //                                      && u.Contrasena == _utilidades.EncriptarSHA256(loginDto.Contrasena));
        //        if (user == null)
        //        {
        //            return (false, "");        //        }
        //        // Assuming GenerarJwt method generates a JWT token for the user
        //        string token = _utilidades.GenerarJwt(user);
        //        return (true, token);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle or log the exception appropriately
        //        throw new InvalidOperationException("An error occurred during login.", ex);
        //    }
        //}

        public async Task<(bool IsSuccess, AuthResponseDto AuthResponse)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == loginDto.NombreUsuario
                                              && u.Contrasena == _utilidades.EncriptarSHA256(loginDto.Contrasena));

                if (user == null)
                {
                    return (false, null);
                }

                // Suponiendo que el método GenerarJwt genera un token JWT para el usuario
                string token = _utilidades.GenerarJwt(user);

                var userDto = new UsuarioDto
                {
                    UsuarioId = user.UsuarioId,
                    NombreUsuario = user.NombreUsuario,
                    Email = user.Email
                    // Mapear otras propiedades del usuario si es necesario
                };

                var authResponse = new AuthResponseDto
                {
                    Token = token,
                    Usuario = userDto
                };

                return (true, authResponse);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error durante el inicio de sesión.", ex);
            }
        }


        public async Task RegistroAsync(Usuario usuario)
        {
            try
            {
                var existingUser = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario);
                if (existingUser)
                {
                    throw new InvalidOperationException("El Nombre de Usuario ya está en uso.");
                }

                var existingEmail = await _context.Usuarios
                    .AnyAsync(u => u.Email == usuario.Email);
                if (existingEmail)
                {
                    throw new InvalidOperationException("El Email ya está en uso.");
                }

                var userModel = new Usuario
                {
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Contrasena = _utilidades.EncriptarSHA256(usuario.Contrasena),
                    NombreUsuario = usuario.NombreUsuario,
                    FechaRegistro = usuario.FechaRegistro,
                };

                await _context.Usuarios.AddAsync(userModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al registrar el usuario. Asegúrese de que los datos sean únicos y válidos.", ex);
            }
        }




    }
}
