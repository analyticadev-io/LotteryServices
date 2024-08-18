using System;
using System.Collections.Generic;

namespace BasicBackendTemplate.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Rol> Rols { get; set; } = new List<Rol>();
}
