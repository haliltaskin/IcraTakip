using IcraTakipProgrami.Models;
using IcraTakipProgrami.Models.IcraTakip;
using IcraTakipProgrami.Models.Ihtar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IcraTakipProgrami.Controllers
{
    public class IcraTakibiController:Controller
    {
        private readonly StajProjesiContext _context;
        private IHttpContextAccessor _httpContextAccessor;



        public IcraTakibiController(StajProjesiContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var icraTakibiData = await (from icra in _context.IcraTakibis
                                        join musteri in _context.Musterilers on icra.Borclu equals musteri.Id
                                        join urun in _context.Urunlers on icra.IhtarKonusuUrun equals urun.Id
                                        join avukat in _context.Avukatlars on icra.Avukat equals avukat.Id
                                        join ihtar in _context.Ihtarlars on icra.Borclu equals ihtar.Borclu
                                        select new IcraTakipViewModel
                                        {
                                            Id = icra.Id,
                                            Borclu = musteri.Id,
                                            IhtarKonusuUrun = urun.Id,
                                            Avukat = avukat.Id,
                                            BorcluAd = musteri.Ad + " " + musteri.Soyad,
                                            AvukatAd = avukat.Ad + " " + avukat.Soyad,
                                            IhtarKonusuUrunAd=urun.UrunAdi,
                                            TakipTarihi = icra.TakipTarihi,
                                            TakipTipi = icra.TakipTipi,
                                            Ihtarlar = ihtar.Id,
                                            IhtarlarAd=ihtar.NoterAd,
                                            IcraMudurlugu = icra.IcraMudurlugu
                                            // Diğer özellikleri buraya ekleyebilirsiniz
                                        }).ToListAsync();

            return View(icraTakibiData);
        }





        public async Task<IActionResult> CreateModal()
        {
            // Müşteri verilerinin alınması ve SelectList'in oluşturulması
            var musteriListesi = await _context.Musterilers.ToListAsync();
            var musteriSelectList = new SelectList(musteriListesi, "Id", "Ad");

            // Avukat verilerinin alınması ve SelectList'in oluşturulması
            var avukatListesi = await _context.Avukatlars.ToListAsync();
            var avukatSelectList = new SelectList(avukatListesi, "Id", "Ad");

            // Urun verilerinin alınması ve SelectList'in oluşturulması
            var urunListesi = await _context.Urunlers.ToListAsync();
            var urunSelectList = new SelectList(urunListesi, "Id", "Ad");


            // ViewModel oluşturulması ve SelectList'lerin atanması
            var model = new IcraTakipCreateViewModel
            {
                MusteriEkleListesi = musteriSelectList,
                AvukatEkleListesi = avukatSelectList,
                UrunEkleListesi= urunSelectList
            };

            // Partial view'in döndürülmesi
            return PartialView("Create", model);
        }


        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {
            var musteriListesi = await _context.Musterilers.ToListAsync();
            var musteriSelectList = new SelectList(musteriListesi, "Id", "Ad");

            var avukatListesi = await _context.Avukatlars.ToListAsync();
            var avukatSelectList = new SelectList(avukatListesi, "Id", "Ad");


            var urunListesi = await _context.Urunlers.ToListAsync();
            var urunSelectList = new SelectList(urunListesi, "Id", "Ad");


            var icra = await _context.IcraTakibis.FindAsync(id);
            if (icra == null)
            {
                return NotFound();
            }

            var model = new IcraTakipCreateViewModel
            {
                Borclu = icra.Borclu,
                Avukat = icra.Avukat,
                TakipTarihi = icra.TakipTarihi,
                TakipTipi = icra.TakipTipi,
                Ihtarlar = icra.Ihtarlar,
                IhtarKonusuUrun = icra.IhtarKonusuUrun,
                IcraMudurlugu = icra.IcraMudurlugu,
                MusteriEkleListesi = musteriSelectList,
                AvukatEkleListesi = avukatSelectList,
                UrunEkleListesi= urunSelectList
            };

            return PartialView("Edit", model);
        }



        public async Task<IActionResult> Create()
        {
            ViewBag.Musteriler = new SelectList(await _context.Musterilers.ToListAsync(), "Id", "Ad");
            ViewBag.Avukatlar = new SelectList(await _context.Avukatlars.ToListAsync(), "Id", "Ad");

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Create(IcraTakipViewModel model)
        {

            var icra = new IcraTakibi
            {
                Borclu = model.Borclu,
                Avukat = model.Avukat,
                TakipTarihi = model.TakipTarihi,
                TakipTipi = model.TakipTipi,
                Ihtarlar = model.Ihtarlar,
                IhtarKonusuUrun = model.IhtarKonusuUrun,
                IcraMudurlugu = model.IcraMudurlugu,

            };
            _context.IcraTakibis.Add(icra);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "IcraTakibi");
        }

        [HttpGet]
        public async Task<JsonResult> GetUrunler(int musteriId)
        {
            var urunler = await _context.Urunlers.Where(u => u.MusteriId == musteriId).ToListAsync();
            var urunViewModels = urunler.Select(u => new { value = u.Id, text = u.UrunAdi }).ToList();
            return Json(urunViewModels);
        }


        [HttpGet]
        public async Task<JsonResult> GetAvukatlarByUrun(int urunId)
        {
            var urun = await _context.Urunlers
                                     .Include(u => u.Avukat)
                                     .FirstOrDefaultAsync(u => u.Id == urunId);

            if (urun == null)
            {
                return Json(null);
            }

            var avukatlar = new[] { new { value = urun.Avukat.Id, text = urun.Avukat.Ad } };

            return Json(avukatlar);
        }


        [HttpGet]
        public async Task<JsonResult> GetIhtarlarByUrun(int urunId)
        {
            var ihtarlar = await _context.Ihtarlars.Where(i => i.MusteriUrunleri == urunId).ToListAsync();
            var ihtarViewModels = ihtarlar.Select(i => new { value = i.Id, text = i.NoterAd }).ToList();
            return Json(ihtarViewModels);
        }








        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ıcraTakibi = await _context.IcraTakibis.FindAsync(id);

            if (ıcraTakibi == null)
            {
                return NotFound();
            }

            return View(ıcraTakibi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IcraTakibi model)
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
                    if (_context.IcraTakibis.Any(a => a.Id == model.Id))
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
            var ıcraTakibi = await _context.IcraTakibis.FindAsync(id);
            if (ıcraTakibi == null)
            {
                return NotFound();
            }
            return View(ıcraTakibi);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var ıcraTakibi = await _context.IcraTakibis.FindAsync(id);
            if (ıcraTakibi == null)
            {
                return NotFound();
            }
            _context.IcraTakibis.Remove(ıcraTakibi);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


    }
}
