using System;
using System.Collections.Generic;

namespace IcraTakipProgrami.Models;

public partial class Avukatlar
{
    public int Id { get; set; }

    public string Ad{ get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string? KullaniciAdi { get; set; }

    public string Tckn { get; set; } = null!;

    public string VergiNo { get; set; } = null!;

    public string CepNo { get; set; } = null!;

    public string? AvansHesapSubesi { get; set; }

    public string? AvansHesapNo { get; set; }

    public string? AvukatTipi { get; set; }

    public string? VadesizHesapSubesi { get; set; }

    public string? VadesizHesapNo { get; set; }

    public string EmailAdres { get; set; } = null!;

    public string? TamAdres { get; set; }

    public bool HesapAktifMi { get; set; }

    public DateOnly? EklenmeTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public bool Silindimi { get; set; }

    public string? EkleyenKullanici { get; set; }

    public string? GuncelleyenKullanici { get; set; }

    public virtual ICollection<Urunler> Urunlers { get; set; } = new List<Urunler>();
}
