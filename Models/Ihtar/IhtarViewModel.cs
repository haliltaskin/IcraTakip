namespace IcraTakipProgrami.Models.Ihtar
{
    public class IhtarViewModel
    {
        public int Id { get; set; }

        public int IhtarNo { get; set; }

        public int? Borclu { get; set; }
        public string? BorcluAd { get; set; }


        public int? MusteriUrunleri { get; set; }
        public string MusteriUrun { get; set; }


        public string? NoterAd { get; set; }

        public DateOnly IhtarTarih { get; set; }

        public string IhtarnameSuresi { get; set; } = null!;

        public DateOnly TebligTarihi { get; set; }

        public DateOnly IhtarTebligGirisTarih { get; set; }

        public DateOnly? KatTarih { get; set; }

        public decimal IhtarnameNakitTutarı { get; set; }

        public decimal? IhtarnameGayriNakitTutarı { get; set; }

        public DateOnly? EklenmeTarihi { get; set; }

        public DateTime? GuncellenmeTarihi { get; set; }

        public bool Silindimi { get; set; }

        public string EkleyenKullanici { get; set; } = null!;

        public string? GuncelleyenKullanici { get; set; }

        public virtual ICollection<IcraTakibi> IcraTakibis { get; set; } = new List<IcraTakibi>();

        public virtual Urunler? MusteriUrunleriNavigation { get; set; }
    }
}
