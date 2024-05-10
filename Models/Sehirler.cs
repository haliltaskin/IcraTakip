using System;
using System.Collections.Generic;

namespace IcraTakipProgrami.Models;

public partial class Sehirler
{
    public int Id { get; set; }

    public string? Sehiradi { get; set; }

    public virtual ICollection<Ilceler> Ilcelers { get; set; } = new List<Ilceler>();

    public virtual ICollection<Musteriler> Musterilers { get; set; } = new List<Musteriler>();
}
