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
        private readonly IRol _rolService;

        public ServiceLogin(LoteriaDbContext context, Utilidades utilidades, IRol rolService)
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
                    .Include(u=>u.Boletos)
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
                    Boletos = user.Boletos.Select(b => new Boleto
                    {
                        // Mapea las propiedades de Boleto a BoletoDto
                        BoletoId = b.BoletoId,
                        // Otros campos
                    }).ToList(),
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

                var rol = await _context.Rols.FindAsync(3); // Asume que tienes una entidad `Role` en tu contexto con ID 3
                if (rol != null)
                {
                    userModel.Rols.Add(rol);
                }

                await _context.Usuarios.AddAsync(userModel);              
               
                await _context.SaveChangesAsync();
                /*
                * Cambiar el IdRol Por el id especifico del rol usuario para
                * que todos los usuario al registrarse tengan el rool por defecto user
                */
                //var userId = userModel.UsuarioId;
                //await _rolService.AsignarRolAUsuarioAsync(usuario.UsuarioId, 3);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al registrar el usuario. Asegúrese de que los datos sean únicos y válidos.", ex);
            }
        }




    }
}
