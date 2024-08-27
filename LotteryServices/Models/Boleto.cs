using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LotteryServices.Models;

public partial class Boleto
{
    public int BoletoId { get; set; }

    public int UsuarioId { get; set; }

    public int SorteoId { get; set; }

    public DateTime? FechaCompra { get; set; }

    [JsonIgnore]
    public virtual ICollection<NumerosBoleto> NumerosBoletos { get; set; } = new List<NumerosBoleto>();
    [JsonIgnore]
    public virtual Sorteo Sorteo { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario Usuario { get; set; } = null!;
}
