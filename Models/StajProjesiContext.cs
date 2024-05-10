using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IcraTakipProgrami.Models;

public partial class StajProjesiContext : DbContext
{
    public StajProjesiContext()
    {
    }

    public StajProjesiContext(DbContextOptions<StajProjesiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Avukatlar> Avukatlars { get; set; }

    public virtual DbSet<IcraTakibi> IcraTakibis { get; set; }

    public virtual DbSet<Ihtarlar> Ihtarlars { get; set; }

    public virtual DbSet<Ilceler> Ilcelers { get; set; }

    public virtual DbSet<Musteriler> Musterilers { get; set; }

    public virtual DbSet<Sehirler> Sehirlers { get; set; }

    public virtual DbSet<Urunler> Urunlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DFDM6QH\\SQLEXPRESS;Database=StajProjesi;TrustServerCertificate=True;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Avukatlar>(entity =>
        {
            entity.ToTable("AVUKATLAR");

            entity.Property(e => e.Ad)
                .HasMaxLength(250)
                .IsFixedLength();
            entity.Property(e => e.AvansHesapNo)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("Avans_Hesap_No");
            entity.Property(e => e.AvansHesapSubesi)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("Avans_Hesap_Subesi");
            entity.Property(e => e.AvukatTipi)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("Avukat_Tipi");
            entity.Property(e => e.CepNo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Cep_No");
            entity.Property(e => e.EklenmeTarihi).HasColumnName("Eklenme_Tarihi");
            entity.Property(e => e.EkleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmailAdres)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Email_Adres");
            entity.Property(e => e.GuncellenmeTarihi)
                .HasColumnType("datetime")
                .HasColumnName("Guncellenme_Tarihi");
            entity.Property(e => e.GuncelleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HesapAktifMi).HasColumnName("Hesap_Aktif_mi");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Kullanici_Adi");
            entity.Property(e => e.Soyad)
                .HasMaxLength(250)
                .IsFixedLength();
            entity.Property(e => e.TamAdres)
                .HasMaxLength(400)
                .IsFixedLength()
                .HasColumnName("Tam_Adres");
            entity.Property(e => e.Tckn)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TCKN");
            entity.Property(e => e.VadesizHesapNo)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("Vadesiz_Hesap_No");
            entity.Property(e => e.VadesizHesapSubesi)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("Vadesiz_Hesap_Subesi");
            entity.Property(e => e.VergiNo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Vergi_No");
        });

        modelBuilder.Entity<IcraTakibi>(entity =>
        {
            entity.ToTable("ICRA_TAKIBI");

            entity.Property(e => e.EklenmeTarihi).HasColumnName("Eklenme_Tarihi");
            entity.Property(e => e.EkleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GuncellenmeTarihi)
                .HasColumnType("datetime")
                .HasColumnName("Guncellenme_Tarihi");
            entity.Property(e => e.GuncelleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IcraMudurlugu)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Icra_Mudurlugu");
            entity.Property(e => e.IcraNo).HasColumnName("Icra_No");
            entity.Property(e => e.IhtarKonusuUrun).HasColumnName("Ihtar_Konusu_Urun");
            entity.Property(e => e.TakipTarihi).HasColumnName("Takip_Tarihi");
            entity.Property(e => e.TakipTipi)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("Takip_Tipi");

            entity.HasOne(d => d.IhtarlarNavigation).WithMany(p => p.IcraTakibis)
                .HasForeignKey(d => d.Ihtarlar)
                .HasConstraintName("FK_ICRA_TAKIBI_IHTARLAR");
        });

        modelBuilder.Entity<Ihtarlar>(entity =>
        {
            entity.ToTable("IHTARLAR");

            entity.Property(e => e.EklenmeTarihi).HasColumnName("Eklenme_Tarihi");
            entity.Property(e => e.EkleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GuncellenmeTarihi)
                .HasColumnType("datetime")
                .HasColumnName("Guncellenme_Tarihi");
            entity.Property(e => e.GuncelleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IhtarNo).HasColumnName("Ihtar_No");
            entity.Property(e => e.IhtarTarih).HasColumnName("Ihtar_Tarih");
            entity.Property(e => e.IhtarTebligGirisTarih).HasColumnName("Ihtar_Teblig_Giris_Tarih");
            entity.Property(e => e.IhtarnameGayriNakitTutarı)
                .HasColumnType("money")
                .HasColumnName("Ihtarname_Gayri_Nakit_Tutarı");
            entity.Property(e => e.IhtarnameNakitTutarı)
                .HasColumnType("money")
                .HasColumnName("Ihtarname_Nakit_Tutarı");
            entity.Property(e => e.IhtarnameSuresi)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Ihtarname_Suresi");
            entity.Property(e => e.KatTarih).HasColumnName("Kat_Tarih");
            entity.Property(e => e.MusteriUrunleri).HasColumnName("Musteri_Urunleri");
            entity.Property(e => e.NoterAd)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Noter_Ad");
            entity.Property(e => e.TebligTarihi).HasColumnName("Teblig_Tarihi");

            entity.HasOne(d => d.MusteriUrunleriNavigation).WithMany(p => p.Ihtarlars)
                .HasForeignKey(d => d.MusteriUrunleri)
                .HasConstraintName("FK_IHTARLAR_URUNLER");
        });

        modelBuilder.Entity<Ilceler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ILCELER__3213E83F755D82AB");

            entity.ToTable("ILCELER");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ilceadi)
                .HasMaxLength(255)
                .HasColumnName("ilceadi");
            entity.Property(e => e.Sehirid).HasColumnName("sehirid");

            entity.HasOne(d => d.Sehir).WithMany(p => p.Ilcelers)
                .HasForeignKey(d => d.Sehirid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ILCELER_SEHIRLER");
        });

        modelBuilder.Entity<Musteriler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Musteriler");

            entity.ToTable("MUSTERILER");

            entity.Property(e => e.Ad)
                .HasMaxLength(250)
                .IsFixedLength();
            entity.Property(e => e.AnneAd)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Anne_Ad");
            entity.Property(e => e.BabaAd)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Baba_Ad");
            entity.Property(e => e.Cinsiyet)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.DogumTarihi).HasColumnName("Dogum_Tarihi");
            entity.Property(e => e.DogumYeri)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Dogum_Yeri");
            entity.Property(e => e.EklenmeTarihi).HasColumnName("Eklenme_Tarihi");
            entity.Property(e => e.EkleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GuncellenmeTarihi)
                .HasColumnType("datetime")
                .HasColumnName("Guncellenme_Tarihi");
            entity.Property(e => e.GuncelleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HayattaMı).HasColumnName("Hayatta_mı");
            entity.Property(e => e.MistralMusteriTipi).HasColumnName("Mistral_Musteri_Tipi");
            entity.Property(e => e.MusteriNo).HasColumnName("Musteri_No");
            entity.Property(e => e.Semt)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Soyad)
                .HasMaxLength(250)
                .IsFixedLength();
            entity.Property(e => e.Tckn)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TCKN");

            entity.HasOne(d => d.SehirNavigation).WithMany(p => p.Musterilers)
                .HasForeignKey(d => d.Sehir)
                .HasConstraintName("FK_MUSTERILER_SEHIRLER");
        });

        modelBuilder.Entity<Sehirler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEHIRLER__3213E83F82D68D5C");

            entity.ToTable("SEHIRLER");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Sehiradi)
                .HasMaxLength(255)
                .HasColumnName("sehiradi");
        });

        modelBuilder.Entity<Urunler>(entity =>
        {
            entity.ToTable("URUNLER");

            entity.Property(e => e.AvukatId).HasColumnName("Avukat_Id");
            entity.Property(e => e.AylikFaizOrani)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Aylik_Faiz_Orani");
            entity.Property(e => e.DovizTipi)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Doviz_Tipi");
            entity.Property(e => e.EklenmeTarihi).HasColumnName("Eklenme_Tarihi");
            entity.Property(e => e.EkleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FaizBakiyesi)
                .HasColumnType("money")
                .HasColumnName("Faiz_Bakiyesi");
            entity.Property(e => e.FaizMudiNo)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Faiz_Mudi_No");
            entity.Property(e => e.GuncellenmeTarihi)
                .HasColumnType("datetime")
                .HasColumnName("Guncellenme_Tarihi");
            entity.Property(e => e.GuncelleyenKullanici)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.KrediMudiNo)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Kredi_Mudi_No");
            entity.Property(e => e.MasrafBakiyesi)
                .HasColumnType("money")
                .HasColumnName("Masraf_Bakiyesi");
            entity.Property(e => e.MasrafMudiNo)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Masraf_Mudi_No");
            entity.Property(e => e.MusteriId).HasColumnName("Musteri_Id");
            entity.Property(e => e.TakipBirimKodu)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Takip_Birim_Kodu");
            entity.Property(e => e.TakipMiktari)
                .HasColumnType("money")
                .HasColumnName("Takip_Miktari");
            entity.Property(e => e.TakipMudiNo)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Takip_Mudi_No");
            entity.Property(e => e.TakipTarihi).HasColumnName("Takip_Tarihi");
            entity.Property(e => e.UrunAdi)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Urun_Adi");

            entity.HasOne(d => d.Avukat).WithMany(p => p.Urunlers)
                .HasForeignKey(d => d.AvukatId)
                .HasConstraintName("FK_URUNLER_AVUKATLAR");

            entity.HasOne(d => d.Musteri).WithMany(p => p.Urunlers)
                .HasForeignKey(d => d.MusteriId)
                .HasConstraintName("FK_URUNLER_MUSTERILER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
