using System;
using System.Collections.Generic;

namespace LotteryServices.Models;

public partial class Rol
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Permiso> Permisos { get; } = new List<Permiso>();

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
