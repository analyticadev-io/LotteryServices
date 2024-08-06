using System;
using System.Collections.Generic;

namespace LotteryServices.Models;

public partial class NumerosSorteo
{
    public int NumeroSorteoId { get; set; }

    public int SorteoId { get; set; }

    public int Numero { get; set; }

    public virtual Sorteo Sorteo { get; set; } = null!;
}
