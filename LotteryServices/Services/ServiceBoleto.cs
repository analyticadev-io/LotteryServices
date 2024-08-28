using LotteryServices.Dtos;
using LotteryServices.Interfaces;
using LotteryServices.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryServices.Services
{
    public class ServiceBoleto : IBoleto
    {
        private readonly LoteriaDbContext _context;
        private readonly LoteriaDbContext _servicioEncriptado;

        public ServiceBoleto(LoteriaDbContext context, LoteriaDbContext servicioEncriptado)
        {
            _context = context;
            _servicioEncriptado = servicioEncriptado;
        }

        public async Task<Boleto> AddBoletoAsync(Boleto boleto)
        {
            var existingBoletoForSorteo = _context.Sorteos
        .Include(s => s.Boletos)
        .Where(s => s.SorteoId == boleto.SorteoId)
        .SelectMany(s => s.Boletos)
        .Any(b => b.NumerosBoletos.Any(nb => boleto.NumerosBoletos.Select(bn => bn.Numero).Contains(nb.Numero)));


            if (existingBoletoForSorteo)
            {
                // Opcional: Lanzar una excepción o retornar null si ya existe
                throw new InvalidOperationException("Ya existe un boleto con esos números en el sorteo.");
            }

            var newBoleto = new Boleto{
                UsuarioId = boleto.UsuarioId,
                SorteoId = boleto.SorteoId                
            };
            newBoleto.NumerosBoletos=boleto.NumerosBoletos;
            _context.Boletos.Add(newBoleto);
            await _context.SaveChangesAsync();
            return newBoleto;
        }

        public Task<bool> DeleteBoletoAsync(int boleto)
        {
            throw new NotImplementedException();
        }

        public Task<Boleto> EditBoletoAsync(Boleto boleto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Boleto>> GetBoletosAsync()
        {
            return await _context.Boletos.ToListAsync();
        }

        public async Task<Boleto> GetBoletoByNumAsync(int numeroBoleto)
        {
            var existingBoleto = await _context.Boletos
                .FirstOrDefaultAsync(b => b.NumerosBoletos.Any(nb => nb.Numero == numeroBoleto));

            return existingBoleto;
        }

        ////public async Task<ICollection<Boleto>> GetAllBoletoByUserAsync(int userId)
        ////{
        ////    // Obtiene el usuario con sus boletos asociados
        ////    var user = await _context.Usuarios
        ////       .Include(u => u.Boletos)
        ////       .ThenInclude(b => b.NumerosBoletos)// Incluye los boletos asociados al usuario
        ////       .Include(u => u.Boletos)
        ////       .ThenInclude(b => b.Sorteo)
        ////       .FirstOrDefaultAsync(u => u.UsuarioId == userId);

        ////    if (user == null)
        ////    {
        ////        // Devuelve una colección vacía si el usuario no se encuentra
        ////        return new List<Boleto>();
        ////    }

        ////    var boletos = user.Boletos.ToList();

        ////    // Retorna la colección de boletos del usuario
        ////    return boletos;
        ////}


        public async Task<ICollection<Boleto>> GetAllBoletoByUserAsync(int userId)
        {
            // Obtiene los boletos asociados al usuario con los sorteos relacionados
            var boletos = await _context.Boletos
                .Include(b => b.NumerosBoletos) // Incluye los números de los boletos
                .Include(b => b.Sorteo) // Incluye el sorteo asociado
                .ThenInclude(b=>b.NumerosSorteos)
                .Where(b => b.UsuarioId == userId) // Filtra por el usuario
                .ToListAsync();

            // Retorna la colección de boletos del usuario
            return boletos;
        }


        public async Task<ICollection<Boleto>> GetBoletoActiveByUserAsync(int userId)
        {
            // Obtiene el usuario con sus boletos asociados
            var user = await _context.Usuarios
               .Include(u => u.Boletos)
               .ThenInclude(b=>b.NumerosBoletos)// Incluye los boletos asociados al usuario
               .Include(u => u.Boletos)
               .ThenInclude(b => b.Sorteo)
               .FirstOrDefaultAsync(u => u.UsuarioId == userId);

            if (user == null)
            {
                // Devuelve una colección vacía si el usuario no se encuentra
                return new List<Boleto>();
            }

            var boletosActivos = user.Boletos.Where(b => b.Sorteo != null && b.Sorteo.Status=="active").ToList();

            // Retorna la colección de boletos del usuario
            return boletosActivos;
        }

        public async Task<ICollection<Boleto>> GetBoletoCompleteByUserAsync(int userId)
        {
            // Obtiene el usuario con sus boletos asociados
            var user = await _context.Usuarios
               .Include(u => u.Boletos)
               .ThenInclude(b => b.NumerosBoletos)// Incluye los boletos asociados al usuario
               .Include(u => u.Boletos)
               .ThenInclude(b => b.Sorteo)
               .FirstOrDefaultAsync(u => u.UsuarioId == userId);

            if (user == null)
            {
                // Devuelve una colección vacía si el usuario no se encuentra
                return new List<Boleto>();
            }

            var boletosActivos = user.Boletos.Where(b => b.Sorteo != null && b.Sorteo.Status == "complete").ToList();

            // Retorna la colección de boletos del usuario
            return boletosActivos;
        }

    }
}
