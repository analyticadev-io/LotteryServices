using System;
using System.Collections.Generic;

namespace LotteryServices.Models;

public partial class NumerosBoleto
{
    public int NumeroBoletoId { get; set; }

    public int BoletoId { get; set; }

    public int Numero { get; set; }

    public virtual Boleto Boleto { get; set; } = null!;
}
