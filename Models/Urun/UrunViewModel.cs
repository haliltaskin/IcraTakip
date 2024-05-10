namespace IcraTakipProgrami.Models.Urun
{
    public class UrunViewModel
    {
        public int Id { get; set; }

        public int? MusteriId { get; set; }
        public string MusteriAd { get; set; }


        public int? AvukatId { get; set; }
        public string AvukatAd { get; set; }

        public string UrunAdi { get; set; } = null!;

        public string? KrediMudiNo { get; set; }

        public decimal? TakipMiktari { get; set; }

        public string? DovizTipi { get; set; }

        public string? AylikFaizOrani { get; set; }

        public DateOnly? TakipTarihi { get; set; }

        public string? MasrafMudiNo { get; set; }

        public decimal? MasrafBakiyesi { get; set; }

        public string? FaizMudiNo { get; set; }

        public decimal? FaizBakiyesi { get; set; }

        public string? TakipBirimKodu { get; set; }

        public string? TakipMudiNo { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public DateTime? GuncellenmeTarihi { get; set; }

        public bool Silindimi { get; set; }

        public string EkleyenKullanici { get; set; } = null!;

        public string? GuncelleyenKullanici { get; set; }

        public virtual Avukatlar? Avukat { get; set; }

        public virtual ICollection<Ihtarlar> Ihtarlars { get; set; } = new List<Ihtarlar>();

        public virtual Musteriler? Musteri { get; set; }
    }
}
