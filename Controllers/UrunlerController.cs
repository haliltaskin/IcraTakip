using IcraTakipProgrami.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IcraTakipProgrami.Models.Urun;
using System.Security.Claims;


namespace IcraTakipProgrami.Controllers
{
    public class UrunlerController : Controller
    {
        private readonly StajProjesiContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public UrunlerController(StajProjesiContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {

            //var kk = await _context.Urunlers.ToListAsync();

            var td = from urunler in _context.Urunlers
                     join musteri in _context.Musterilers on urunler.MusteriId equals musteri.Id 
                     join avukat in _context.Avukatlars on urunler.AvukatId equals avukat.Id
                     // where s.Entity_ID = getEntity
                     select new UrunViewModel
                     {
                         Id =urunler.Id,
                         MusteriId =musteri.Id,
                         MusteriAd = musteri.Ad + " " + musteri.Soyad,
                         AvukatAd = avukat.Ad + " " + avukat.Soyad,
                         AvukatId = avukat.Id,
                         UrunAdi = urunler.UrunAdi,
                         KrediMudiNo = urunler.KrediMudiNo,
                         TakipMiktari=urunler.TakipMiktari,
                         DovizTipi=urunler.DovizTipi,
                         AylikFaizOrani = urunler.AylikFaizOrani,
                         TakipTarihi = urunler.TakipTarihi,
                         MasrafMudiNo = urunler.MasrafMudiNo,
                         MasrafBakiyesi = urunler.MasrafBakiyesi,
                         FaizMudiNo = urunler.FaizMudiNo,
                         FaizBakiyesi = urunler.FaizBakiyesi,
                         TakipBirimKodu=urunler.TakipBirimKodu,
                         TakipMudiNo=urunler.TakipMudiNo,
                     };
            return View(td);
        }


        //public async Task<IActionResult> CreateModal()
        //{
        //    var td = (from musteri in _context.Musterilers
        //              select new
        //              {
        //                  Id = musteri.Id,
        //                  MusteriAd = musteri.Ad,
        //                  MusteriSoyad = musteri.Soyad,
        //              }).ToList();

        //    var model = new UrunlerCreateViewModel();

        //    model.MusteriEkleListesi = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
        //    model.AvukatEkleListesi = new SelectList(await _context.Avukatlars.ToListAsync(), "Id", "Ad");



        //    return PartialView("Create", model);
        //}

        public async Task<IActionResult> CreateModal()
        {
            // Müşteri verilerinin alınması ve SelectList'in oluşturulması
            var musteriListesi = await _context.Musterilers.ToListAsync();
            var musteriSelectList = new SelectList(musteriListesi, "Id", "Ad");

            // Avukat verilerinin alınması ve SelectList'in oluşturulması
            var avukatListesi = await _context.Avukatlars.ToListAsync();
            var avukatSelectList = new SelectList(avukatListesi, "Id", "Ad");

            // ViewModel oluşturulması ve SelectList'lerin atanması
            var model = new UrunlerCreateViewModel
            {
                MusteriEkleListesi = musteriSelectList,
                AvukatEkleListesi = avukatSelectList
            };

            // Partial view'in döndürülmesi
            return PartialView("Create", model);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            ViewBag.Avukatlar = new SelectList(await _context.Avukatlars.ToListAsync(), "Id", "Ad");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(UrunViewModel model)
        {
            
            var urun = new Urunler
            {
                MusteriId = model.MusteriId,
                AvukatId = model.AvukatId,
                UrunAdi = model.UrunAdi,
                KrediMudiNo = model.KrediMudiNo,
                TakipMiktari = model.TakipMiktari,
                DovizTipi = model.DovizTipi,
                AylikFaizOrani = model.AylikFaizOrani,
                TakipTarihi = model.TakipTarihi,
                MasrafMudiNo = model.MasrafMudiNo,
                MasrafBakiyesi = model.MasrafBakiyesi,
                FaizMudiNo = model.FaizMudiNo,
                FaizBakiyesi = model.FaizBakiyesi,
                TakipBirimKodu = model.TakipBirimKodu,
                TakipMudiNo = model.TakipMudiNo,
                
            };
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.EkleyenKullanici = userId;
            model.EklenmeTarihi = DateTime.Now;
            _context.Urunlers.Add(urun);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Urunler");
        }
       


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            
            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            ViewBag.Avukatlar = new SelectList(await _context.Avukatlars.ToListAsync(), "Id", "Ad");
            if (id == null)
            {
                return NotFound();
            }

            var Urun = await _context.Urunlers.FindAsync(id);

            if (Urun == null)
            {
                return NotFound();
            }

            return View(Urun);
        }



        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {


            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            ViewBag.Avukatlar = new SelectList(await _context.Avukatlars.ToListAsync(), "Id", "Ad");

            if (id == null)
            {
                return NotFound();
            }


            var urun = await _context.Urunlers.FindAsync(id);
            if (urun == null)
            {
                return NotFound();
            }


            return PartialView("Edit", urun);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Urunler model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    model.GuncelleyenKullanici = userId;
                    model.GuncellenmeTarihi = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Urunlers.Any(a => a.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var urun = await _context.Urunlers.FindAsync(id);
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var urun = await _context.Urunlers.FindAsync(id);
            if (urun == null)
            {
                return NotFound();
            }
            _context.Urunlers.Remove(urun);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
