using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace IcraTakipProgrami.Models;

public partial class Musteriler
{
    public int Id { get; set; }

    public int MusteriNo { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string Tckn { get; set; } = null!;

    public string? DogumYeri { get; set; }

    public DateOnly? DogumTarihi { get; set; }

    public string? Cinsiyet { get; set; }

    public string? BabaAd { get; set; }

    public string? AnneAd { get; set; }

    public int? Sehir { get; set; }

    public int? Ilce { get; set; }

    public string? Semt { get; set; }

    public bool MistralMusteriTipi { get; set; }

    public bool HayattaMı { get; set; }

    public DateOnly? EklenmeTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public bool Silindimi { get; set; }

    public string? EkleyenKullanici { get; set; }

    public string? GuncelleyenKullanici { get; set; }

    public virtual Sehirler? SehirNavigation { get; set; }

    public virtual ICollection<Urunler> Urunlers { get; set; } = new List<Urunler>();


}
