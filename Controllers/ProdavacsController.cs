using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradinataNaBabaRatka.Data;
using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GradinataNaBabaRatka.Controllers
{
    public class ProdavacsController : Controller
    {
        private readonly GradinataNaBabaRatkaContext _context;

        public ProdavacsController(GradinataNaBabaRatkaContext context)
        {
            _context = context;
        }

        // GET: Prodavacs
        public async Task<IActionResult> Index(string searchName, string searchSurname, string searchNationality)
        {
            IQueryable<Prodavac> prodavacs = _context.Prodavac.AsQueryable();
            IQueryable<string> FirstNameQuery = _context.Prodavac.OrderBy(m => m.FirstName).Select(m => m.FirstName).Distinct();

            if (!string.IsNullOrEmpty(searchName))
            {
                prodavacs = prodavacs.Where(s => s.FirstName.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchSurname))
            {
                prodavacs = prodavacs.Where(x => x.LastName.Contains(searchSurname));
            }

            if (!string.IsNullOrEmpty(searchNationality))
            {
                prodavacs = prodavacs.Where(c => c.Nationality.Contains(searchNationality));
            }
            

            return View(await prodavacs.ToListAsync());
        }

        // GET: Prodavacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prodavac == null)
            {
                return NotFound();
            }

            var prodavac = await _context.Prodavac
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodavac == null)
            {
                return NotFound();
            }

            return View(prodavac);
        }

        // GET: Prodavacs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prodavacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate,Nationality,Gender")] Prodavac prodavac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prodavac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prodavac);
        }

        // GET: Prodavacs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prodavac == null)
            {
                return NotFound();
            }

            var prodavac = await _context.Prodavac.FindAsync(id);
            if (prodavac == null)
            {
                return NotFound();
            }
            return View(prodavac);
        }

        // POST: Prodavacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,Nationality,Gender")] Prodavac prodavac)
        {
            if (id != prodavac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prodavac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdavacExists(prodavac.Id))
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
            return View(prodavac);
        }

        // GET: Prodavacs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prodavac == null)
            {
                return NotFound();
            }

            var prodavac = await _context.Prodavac
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodavac == null)
            {
                return NotFound();
            }

            return View(prodavac);
        }

        // POST: Prodavacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prodavac == null)
            {
                return Problem("Entity set 'GradinataNaBabaRatkaContext.Prodavac'  is null.");
            }
            var prodavac = await _context.Prodavac.FindAsync(id);
            if (prodavac != null)
            {
                _context.Prodavac.Remove(prodavac);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdavacExists(int id)
        {
          return (_context.Prodavac?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
