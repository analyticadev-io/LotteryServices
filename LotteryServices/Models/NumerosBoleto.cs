using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LotteryServices.Models;

public partial class NumerosBoleto
{
    public int NumeroBoletoId { get; set; }

    public int BoletoId { get; set; }

    public long Numero { get; set; }
    [JsonIgnore]
    public virtual Boleto Boleto { get; set; } = null!;
}
