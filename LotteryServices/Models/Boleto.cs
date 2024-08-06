using System;
using System.Collections.Generic;

namespace LotteryServices.Models;

public partial class Boleto
{
    public int BoletoId { get; set; }

    public int UsuarioId { get; set; }

    public int SorteoId { get; set; }

    public DateTime? FechaCompra { get; set; }

    public virtual ICollection<NumerosBoleto> NumerosBoletos { get; } = new List<NumerosBoleto>();

    public virtual Sorteo Sorteo { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
