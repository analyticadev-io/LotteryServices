using LotteryServices.Models;

namespace LotteryServices.Dtos
{
    public class RolPermisoResponseDto
    {
        public string RolName { get; set; }
        public List<Permiso> Permisos { get; set; }
    }
}
