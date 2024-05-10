using IcraTakipProgrami.Models;
using IcraTakipProgrami.Models.Avukat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IcraTakipProgrami.Controllers
{
    public class AvukatlarController : Controller
    {
        private readonly StajProjesiContext _context;

        private IHttpContextAccessor _httpContextAccessor;
        public AvukatlarController(StajProjesiContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Avukatlars.ToListAsync());
        }
        public IActionResult CreateModal()
        {
            var model = new AvukatCreateViewModel();

            return PartialView("Create", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Avukatlar model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            model.EkleyenKullanici = userId;
            model.EklenmeTarihi = DateOnly.FromDateTime(DateTime.Now);
            model.Ad = model.Ad.Trim();

            _context.Avukatlars.Add(model);
                 
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Avukatlar");    
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Avukat = await _context.Avukatlars.FindAsync(id);

            if (Avukat == null)
            {
                return NotFound();
            }

            return View(Avukat);
        }

        

        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var avukat = _context.Avukatlars.Find(id);
            if (avukat == null)
            {
                return NotFound();
            }
            return PartialView("Edit", avukat);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Avukatlar model)
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
                    model.Ad = model.Ad.Trim();

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Avukatlars.Any(a => a.Id == model.Id))
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
            var avukat = await _context.Avukatlars.FindAsync(id);
            if (avukat == null)
            {
                return NotFound();
            }
            return View(avukat);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var avukat = await _context.Avukatlars.FindAsync(id);
            if (avukat == null)
            {
                return NotFound();
            }
            _context.Avukatlars.Remove(avukat);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
