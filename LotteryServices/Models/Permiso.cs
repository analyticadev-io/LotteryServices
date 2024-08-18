using System;
using System.Collections.Generic;

namespace BasicBackendTemplate.Models;

public partial class Permiso
{
    public int PermisoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Rol> Rols { get; } = new List<Rol>();

    
}
