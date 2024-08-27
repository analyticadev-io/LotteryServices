using System;
using System.Collections.Generic;

namespace LotteryServices.Models;

public partial class Sorteo
{
    public int SorteoId { get; set; }

    public DateTime FechaSorteo { get; set; }

    public string? Title { get; set; }
    public string? Descripcion { get; set; }
    public string? Status { get; set; }

    public virtual ICollection<Boleto> Boletos { get; } = new List<Boleto>();

    public virtual ICollection<NumerosSorteo> NumerosSorteos { get; } = new List<NumerosSorteo>();
}
