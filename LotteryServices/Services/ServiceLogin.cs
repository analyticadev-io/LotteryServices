using BasicBackendTemplate.Models;
using LotteryServices.Dtos;
using LotteryServices.Interfaces;

using LotteryServices.Utilitys;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Services
{
    public class ServiceLogin : ILogin
    {
        private readonly BasicBackendTemplateContext _context;
        private readonly Utilidades _utilidades;
        private readonly IRol _rolService;

        public ServiceLogin(BasicBackendTemplateContext context, Utilidades utilidades, IRol rolService)
        {
            _context = context;
            _utilidades = utilidades;
            rolService = _rolService;
        }
        public async Task<(bool IsSuccess, AuthResponseDto AuthResponse)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Usuarios
                    .Include(u=>u.Rols)
                    .ThenInclude(r=>r.Permisos)
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
                    Email = user.Email,
                    Nombre=user.Nombre,
                    Rol = user.Rols.Select(r => new Rol
                    {
                        // Mapea las propiedades de Rol a RolDto
                        RolId = r.RolId,
                        Nombre = r.Nombre,
                        Permisos = r.Permisos.Select(p => new Permiso
                        {
                            PermisoId = p.PermisoId,
                            Descripcion = p.Descripcion.ToString(),
                        }).ToList()
                    }).ToList(),
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
                    Rols = new List<Rol>()
                };

                var rol = await _context.Rols.FindAsync(2); // Asume que tienes una entidad `Role` en tu contexto con ID 3
                if (rol != null)
                {
                    userModel.Rols.Add(rol);
                }

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
