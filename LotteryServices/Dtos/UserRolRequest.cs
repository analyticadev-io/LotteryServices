namespace LotteryServices.Dtos
{
    public class UserRolRequest
    {
        public int UsuarioId { get; set; }
        public int RolId { get; set; }

        public int RolIdAEliminar { get; set; } // Rol actual que se reemplazará
    }
}
