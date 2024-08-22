using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LotteryServices.Models;

public partial class Permiso
{
    public int PermisoId { get; set; }

    public string Descripcion { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Rol> Rols { get; } = new List<Rol>();


}
