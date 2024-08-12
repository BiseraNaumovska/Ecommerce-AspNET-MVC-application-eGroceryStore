using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradinataNaBabaRatka.Data;
using GradinataNaBabaRatka.Models;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using GradinataNaBabaRatka.ViewModels;

namespace GradinataNaBabaRatka.Controllers
{
    public class ProizvodsController : Controller
    {
        private readonly GradinataNaBabaRatkaContext _context;
       
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProizvodsController(GradinataNaBabaRatkaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Proizvods
        public async Task<IActionResult> Index(string searchString, string searchGrad)
        {

            var proizvodstoreContext = _context.Proizvod.Include(b => b.Prodavac).Include(b => b.Reviews).Include(b => b.ProizvodGrads).ThenInclude(b => b.Grad);
            var gradoviContext = _context.ProizvodGrad.Include(b => b.Grad).Include(b => b.Proizvod);
            var proizvods = from m in proizvodstoreContext
                        select m;
            var gradovis = from n in gradoviContext select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                proizvods = proizvods.Where(s => s.Ime!.Contains(searchString));

            }

            if (!String.IsNullOrEmpty(searchGrad))
            {
                gradovis = gradovis.Where(s => s.Grad.GradIme!.Contains(searchGrad));
                var innerJoinQuery =
                 from m in proizvods
                 join n in gradovis on m.Id equals n.ProizvodId
                 select new { m };
                ArrayList lista = new ArrayList();

                foreach (var ownerAndPet in innerJoinQuery)
                {

                    lista.Add(ownerAndPet.m);
                }

                ViewBag.lista = lista;
            }

            return View(await proizvods.ToListAsync());
        }
        public async Task<IActionResult> GenreIndex(int? id)
        {
            IQueryable<ProizvodGrad> bookgenres = _context.ProizvodGrad.AsQueryable();
            IQueryable<Proizvod>? qbooks = _context.Proizvod.AsQueryable();

            if (id < 1 || id == null)
            {
                bookgenres = bookgenres.Include(p => p.Proizvod).ThenInclude(p => p.Prodavac);
            }
            else
            {
                bookgenres = bookgenres.Include(p => p.Proizvod).ThenInclude(p => p.Prodavac).Include(p => p.Grad).Where(p => p.GradId == id);
            }
            qbooks = bookgenres.Select(p => p.Proizvod); // added vtoriov include

            return View("~/Views/Proizvods/Index.cshtml", await qbooks.ToListAsync());
        }

        //public async Task<IActionResult> DownloadFile(string downloadUrl)
        //{
        //    var path = Path.Combine(_webHostEnvironment.WebRootPath, "pdfs", downloadUrl);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, "application/pdf", downloadUrl);
        //}

        // GET: Proizvods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proizvod == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvod
                .Include(p => p.Prodavac).Include(b => b.Reviews).Include(p => p.Kupuvacs)
                .Include(b => b.ProizvodGrads).ThenInclude(b => b.Grad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proizvod == null)
            {
                return NotFound();
            }

            return View(proizvod);
        }

        // GET: Proizvods/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProdavacId"] = new SelectList(_context.Prodavac, "Id", "FullName");
            return View();
        }

        // POST: Proizvods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Ime,Data,Zaliha,Opis,Slika,Cena,ProdavacId")] Proizvod proizvod)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(proizvod);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            ViewData["ProdavacId"] = new SelectList(_context.Prodavac, "Id", "FullName", proizvod.ProdavacId);
            return View(proizvod);
        }

        // GET: Proizvods/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proizvod == null)
            {
                return NotFound();
            }

            var proizvod = _context.Proizvod.Where(m => m.Id == id).Include(m => m.ProizvodGrads).First();

            //  var proizvod = await _context.Proizvod.FindAsync(id);
            if (proizvod == null)
            {
                return NotFound();
            }

            var grads = _context.Grad.AsEnumerable();
            grads = grads.OrderBy(s => s.GradIme);
            ProizvodGradEditViewModel viewmodel = new ProizvodGradEditViewModel
            {
                Proizvod = proizvod,
                GradList = new MultiSelectList(grads, "Id", "GradName"),
                SelectedGrads = proizvod.ProizvodGrads.Select(sa => sa.GradId)
            };


            ViewData["ProdavacId"] = new SelectList(_context.Prodavac, "Id", "FullName", proizvod.ProdavacId);
            return View(proizvod);
        }

        // POST: Proizvods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ProizvodGradEditViewModel viewmodel)
        {
            if (id != viewmodel.Proizvod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Proizvod);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> newGenreList = viewmodel.SelectedGrads;
                    IEnumerable<int> prevGenreList = _context.ProizvodGrad.Where(s => s.ProizvodId == id).Select(s => s.GradId);
                    IQueryable<ProizvodGrad> toBeRemoved = _context.ProizvodGrad.Where(s => s.ProizvodId == id);
                    if (newGenreList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GradId));
                        foreach (int genreId in newGenreList)
                        {
                            if (!prevGenreList.Any(s => s == genreId))
                            {
                                _context.ProizvodGrad.Add(new ProizvodGrad { GradId = genreId, ProizvodId = id });
                            }
                        }
                    }
                    _context.ProizvodGrad.RemoveRange(toBeRemoved);
                   

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProizvodExists(viewmodel.Proizvod.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdavacId"] = new SelectList(_context.Set<Prodavac>(), "Id", "FullName", viewmodel.Proizvod.ProdavacId);
            return View(viewmodel);
        }

        // GET: Proizvods/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proizvod == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvod
                .Include(p => p.Prodavac).Include(b => b.Reviews).Include(p => p.Kupuvacs)
                .Include(b => b.ProizvodGrads).ThenInclude(b => b.Grad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proizvod == null)
            {
                return NotFound();
            }

            return View(proizvod);
        }

        // POST: Proizvods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proizvod == null)
            {
                return Problem("Entity set 'GradinataNaBabaRatkaContext.Proizvod'  is null.");
            }
            var proizvod = await _context.Proizvod.FindAsync(id);
            if (proizvod != null)
            {
                _context.Proizvod.Remove(proizvod);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProizvodExists(int id)
        {
          return (_context.Proizvod?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
