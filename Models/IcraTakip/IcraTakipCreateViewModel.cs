﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace IcraTakipProgrami.Models.IcraTakip
{
    public class IcraTakipCreateViewModel
    {
        public int Id { get; set; }

        public int IcraNo { get; set; }

        public int? Borclu { get; set; }

        public int? Avukat { get; set; }

        public DateOnly TakipTarihi { get; set; }

        public string TakipTipi { get; set; } = null!;

        public int? Ihtarlar { get; set; }

        public int? IhtarKonusuUrun { get; set; }

        public string IcraMudurlugu { get; set; } = null!;

        public DateOnly? EklenmeTarihi { get; set; }

        public DateTime? GuncellenmeTarihi { get; set; }

        public bool Silindimi { get; set; }

        public string EkleyenKullanici { get; set; } = null!;

        public string? GuncelleyenKullanici { get; set; }

        public virtual Ihtarlar? IhtarlarNavigation { get; set; }
        public SelectList AvukatEkleListesi { get; set; }
        public SelectList MusteriEkleListesi { get; set; }
        public SelectList UrunEkleListesi { get; set; }
        public SelectList IhtarEkleListesi { get; set; }


    }
}
