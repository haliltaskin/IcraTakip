using IcraTakipProgrami.Models;
using IcraTakipProgrami.Models.Ihtar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IcraTakipProgrami.Controllers
{
    public class IhtarlarController : Controller
    {
        private readonly StajProjesiContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public IhtarlarController(StajProjesiContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Index()
        {

            var td = await (from ihtarlar in _context.Ihtarlars
                            join musteri in _context.Musterilers on ihtarlar.Borclu equals musteri.Id
                            join urun in _context.Urunlers on musteri.Id equals urun.MusteriId
                            // where s.entity_ıd = getentity
                            select new IhtarViewModel
                            {
                                Id = ihtarlar.Id,
                                IhtarNo = ihtarlar.IhtarNo,
                                Borclu = musteri.Id,
                                MusteriUrunleri = urun.Id,
                                BorcluAd = musteri.Ad + " " + musteri.Soyad,
                                MusteriUrun = urun.UrunAdi,
                                NoterAd = ihtarlar.NoterAd,
                                IhtarTarih = ihtarlar.IhtarTarih,
                                IhtarnameSuresi = ihtarlar.IhtarnameSuresi,
                                TebligTarihi = ihtarlar.TebligTarihi,
                                KatTarih = ihtarlar.KatTarih,
                                IhtarnameNakitTutarı = ihtarlar.IhtarnameNakitTutarı,
                                IhtarnameGayriNakitTutarı = ihtarlar.IhtarnameGayriNakitTutarı

                            }).ToListAsync();

            return View(td.DistinctBy(p => p.Id));
            //return View(await _context.Ihtarlars.ToListAsync());
        }



        public async Task<IActionResult> CreateModal()
        {
            // Müşteri verilerinin alınması ve SelectList'in oluşturulması
            var musteriListesi = await _context.Musterilers.ToListAsync();
            var musteriSelectList = new SelectList(musteriListesi, "Id", "Ad");

            // Avukat verilerinin alınması ve SelectList'in oluşturulması
            // var urunListesi = await _context.Urunlers.ToListAsync();
            // var urunSelectList = new SelectList(urunListesi, "Id", "UrunAdi");

            // ViewModel oluşturulması ve SelectList'lerin atanması
            var model = new IhtarCreateViewModel
            {
                MusteriEkleListesi = musteriSelectList,
                // UrunEkleListesi = urunSelectList
            };

            // Partial view'in döndürülmesi
            return PartialView("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {
            var musteriListesi = await _context.Musterilers.ToListAsync();
            var musteriSelectList = new SelectList(musteriListesi, "Id", "Ad");
            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            // ViewBag.Urunler = new SelectList(await _context.Urunlers.ToListAsync(), "Id", "UrunAdi");

            var ihtar = await _context.Ihtarlars.FindAsync(id);
            if (ihtar == null)
            {
                return NotFound();
            }

            var model = new IhtarCreateViewModel
            {
                Id = ihtar.Id,
                Borclu = ihtar.Borclu,
                MusteriUrunleri = ihtar.MusteriUrunleri,
                NoterAd = ihtar.NoterAd,
                IhtarTarih = ihtar.IhtarTarih,
                IhtarnameSuresi = ihtar.IhtarnameSuresi,
                TebligTarihi = ihtar.TebligTarihi,
                IhtarTebligGirisTarih = ihtar.IhtarTebligGirisTarih,
                KatTarih = ihtar.KatTarih,
                IhtarnameNakitTutarı = ihtar.IhtarnameNakitTutarı,
                IhtarnameGayriNakitTutarı = ihtar.IhtarnameGayriNakitTutarı,
                MusteriEkleListesi = musteriSelectList,
            };

            return PartialView("Edit", model);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            ViewBag.Urunler = new SelectList(await _context.Urunlers.ToListAsync(), "Id", "UrunAdi");
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Create(IhtarViewModel model)
        {

            var ıhtar = new Ihtarlar
            {
                Borclu = model.Borclu,
                MusteriUrunleri = model.MusteriUrunleri,
                NoterAd = model.NoterAd,
                IhtarTarih = model.IhtarTarih,
                IhtarnameSuresi = model.IhtarnameSuresi,
                TebligTarihi = model.TebligTarihi,
                IhtarTebligGirisTarih = model.IhtarTebligGirisTarih,
                KatTarih = model.KatTarih,
                IhtarnameNakitTutarı = model.IhtarnameNakitTutarı,
                IhtarnameGayriNakitTutarı = model.IhtarnameGayriNakitTutarı,
            };
            _context.Ihtarlars.Add(ıhtar);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Ihtarlar");
        }

        [HttpGet]
        public async Task<JsonResult> GetUrunler(int musteriId)
        {
            var urunler = await _context.Urunlers.Where(u => u.MusteriId == musteriId).ToListAsync();
            var urunViewModels = urunler.Select(u => new { value = u.Id, text = u.UrunAdi }).ToList();
            return Json(urunViewModels);
        }
        [HttpGet]
        public async Task<JsonResult> GetUrunlerForEdit(int musteriId, int urunId)
        {
            var urunler = await _context.Urunlers.Where(u => u.MusteriId == musteriId).ToListAsync();
            var urunViewModels = urunler.Select(u => new { value = u.Id, text = u.UrunAdi, selected = u.Id == urunId }).ToList();
            return Json(urunViewModels);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var musteriListesi = await _context.Musterilers.ToListAsync();
            var musteriSelectList = new SelectList(musteriListesi, "Id", "Ad");
            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            // ViewBag.Urunler = new SelectList(await _context.Urunlers.ToListAsync(), "Id", "UrunAdi");

            if (id == null)
            {
                return NotFound();
            }

            var ihtarlar = await _context.Ihtarlars.FindAsync(id);

            if (ihtarlar == null)
            {
                return NotFound();
            }

            var model = new IhtarCreateViewModel
            {
                Id = ihtarlar.Id,
                Borclu = ihtarlar.Borclu,
                MusteriUrunleri = ihtarlar.MusteriUrunleri,
                NoterAd = ihtarlar.NoterAd,
                IhtarTarih = ihtarlar.IhtarTarih,
                IhtarnameSuresi = ihtarlar.IhtarnameSuresi,
                TebligTarihi = ihtarlar.TebligTarihi,
                IhtarTebligGirisTarih = ihtarlar.IhtarTebligGirisTarih,
                KatTarih = ihtarlar.KatTarih,
                IhtarnameNakitTutarı = ihtarlar.IhtarnameNakitTutarı,
                IhtarnameGayriNakitTutarı = ihtarlar.IhtarnameGayriNakitTutarı,
                MusteriEkleListesi = musteriSelectList,
            };

            return View(model);
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IhtarCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                // Düzenleme işlemleri
                var ihtarlar = await _context.Ihtarlars.FindAsync(id);
                ihtarlar.Borclu = model.Borclu;
                ihtarlar.MusteriUrunleri = model.MusteriUrunleri;
                ihtarlar.NoterAd = model.NoterAd;
                ihtarlar.IhtarTarih = model.IhtarTarih;
                ihtarlar.IhtarnameSuresi = model.IhtarnameSuresi;
                ihtarlar.TebligTarihi = model.TebligTarihi;
                ihtarlar.IhtarTebligGirisTarih = model.IhtarTebligGirisTarih;
                ihtarlar.KatTarih = model.KatTarih;
                ihtarlar.IhtarnameNakitTutarı = model.IhtarnameNakitTutarı;
                ihtarlar.IhtarnameGayriNakitTutarı = model.IhtarnameGayriNakitTutarı;

                _context.Update(ihtarlar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Ihtarlars.Any(a => a.Id == model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");




            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ihtarlar = await _context.Ihtarlars.FindAsync(id);
            if (ihtarlar == null)
            {
                return NotFound();
            }
            return View(ihtarlar);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var ihtarlar = await _context.Ihtarlars.FindAsync(id);
            if (ihtarlar == null)
            {
                return NotFound();
            }
            _context.Ihtarlars.Remove(ihtarlar);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


    }

}



