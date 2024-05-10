using System;
using System.Collections.Generic;

namespace IcraTakipProgrami.Models;

public partial class Ilceler
{
    public int Id { get; set; }

    public string Ilceadi { get; set; } = null!;

    public int Sehirid { get; set; }

    public virtual Sehirler Sehir { get; set; } = null!;
}
