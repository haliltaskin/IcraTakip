using IcraTakipProgrami.Models;
using IcraTakipProgrami.Models.Ihtar;
using IcraTakipProgrami.Models.Musteri;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IcraTakipPrototip.Controllers
{
    public class MusterilerController : Controller
    {
        private readonly StajProjesiContext _context;

        private IHttpContextAccessor _httpContextAccessor;


        public MusterilerController(StajProjesiContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }


        public IActionResult Create()
        {
            return View();
        }

        public IActionResult IlceSet(int sehirId)
        {
            var ilceListesi = _context.Ilcelers.Where(p => p.Sehirid == sehirId).ToListAsync();

            return Json(new { ilceListesi });
        }

        public IActionResult IlceUpdateSet(int sehirId)
        {
            var ilceListesi = _context.Ilcelers.Where(p => p.Sehirid == sehirId).ToListAsync();

            return Json(new { ilceListesi });
        }

        public async Task<IActionResult> CreateModal()
        {
            var sehirListesi = await _context.Sehirlers.ToListAsync();
            //var ilceListesi = await _context.Ilcelers.Where(p>) .ToListAsync();

            var sehirSelectList = new SelectList(sehirListesi, "Id", "Sehiradi");

            //var ilceSelectList = new SelectList(ilceListesi, "Id", "Ilceadi");


            var model = new MusteriCreateViewModel
            {
                SehirEkleListesi = sehirSelectList,
                //IlceEkleListesi = ilceSelectList
            };
            return PartialView("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {
            var sehirListesi = await _context.Sehirlers.ToListAsync();
            var sehirSelectList = new SelectList(sehirListesi, "Id", "Sehiradi");



            //var avukat = _context.Musterilers.Find(id);
            //if (avukat == null)
            //{
            //    return NotFound();
            //}
            //return PartialView("Edit", avukat);

            var musteri = await _context.Musterilers.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }



            var model = new MusteriCreateViewModel
            {


                Id = musteri.Id,
                Ad = musteri.Ad,
                Soyad = musteri.Soyad,
                Tckn = musteri.Tckn,
                DogumTarihi = musteri.DogumTarihi,
                DogumYeri = musteri.DogumYeri,
                Cinsiyet = musteri.Cinsiyet,
                BabaAd = musteri.BabaAd,
                AnneAd = musteri.AnneAd,
                Sehir = musteri.Sehir,
                Ilce = musteri.Ilce,
                Semt = musteri.Semt,
                MistralMusteriTipi = musteri.MistralMusteriTipi,
                HayattaMı = musteri.HayattaMı,
                SehirEkleListesi = sehirSelectList
            };

            return PartialView("Edit", model);
        }



        public async Task<IActionResult> Index()
        {
            var td = from musteriler in _context.Musterilers
                     join sehir in _context.Sehirlers on musteriler.Sehir equals sehir.Id
                     join ilce in _context.Ilcelers on musteriler.Ilce equals ilce.Id

                     select new MusteriViewModel
                     {
                         Id = musteriler.Id,
                         Ad = musteriler.Ad,
                         Soyad = musteriler.Soyad,
                         Tckn = musteriler.Tckn,
                         DogumTarihi = musteriler.DogumTarihi,
                         DogumYeri = musteriler.DogumYeri,
                         Cinsiyet = musteriler.Cinsiyet,
                         BabaAd = musteriler.BabaAd,
                         AnneAd = musteriler.AnneAd,
                         Sehir = sehir.Id,
                         SehirAd = sehir.Sehiradi,
                         Ilce = ilce.Id,
                         IlceAd = ilce.Ilceadi,
                         Semt = musteriler.Semt,
                         MistralMusteriTipi = musteriler.MistralMusteriTipi,
                         HayattaMı = musteriler.HayattaMı,

                     };

            return View(td);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Musteriler model)
        {
            var musteri = new Musteriler
            {
                Id = model.Id,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Tckn = model.Tckn,
                DogumTarihi = model.DogumTarihi,
                DogumYeri = model.DogumYeri,
                Cinsiyet = model.Cinsiyet,
                BabaAd = model.BabaAd,
                AnneAd = model.AnneAd,
                Sehir = model.Sehir,
                Ilce = model.Ilce,
                Semt = model.Semt,
                MistralMusteriTipi = model.MistralMusteriTipi,
                HayattaMı = model.HayattaMı,

            };

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.EkleyenKullanici = userId;
            model.EklenmeTarihi = DateOnly.FromDateTime(DateTime.Now);

            _context.Musterilers.Add(musteri);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Musteri = await _context.Musterilers.FindAsync(id);



            if (Musteri == null)
            {
                return NotFound();
            }

            return View(Musteri);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Musteriler model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

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
                if (_context.Musterilers.Any(a => a.Id == model.Id))
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
            var musteri = await _context.Musterilers.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var musteri = await _context.Musterilers.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }
            _context.Musterilers.Remove(musteri);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
