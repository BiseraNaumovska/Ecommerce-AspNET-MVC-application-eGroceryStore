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
    public class GradsController : Controller
    {
        private readonly GradinataNaBabaRatkaContext _context;

        public GradsController(GradinataNaBabaRatkaContext context)
        {
            _context = context;
        }

        // GET: Grads
        public async Task<IActionResult> Index()
        {
              return _context.Grad != null ? 
                          View(await _context.Grad.ToListAsync()) :
                          Problem("Entity set 'GradinataNaBabaRatkaContext.Grad'  is null.");
        }

        // GET: Grads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Grad == null)
            {
                return NotFound();
            }

            var grad = await _context.Grad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grad == null)
            {
                return NotFound();
            }

            return View(grad);
        }

        // GET: Grads/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,GradIme")] Grad grad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grad);
        }

        // GET: Grads/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Grad == null)
            {
                return NotFound();
            }

            var grad = await _context.Grad.FindAsync(id);
            if (grad == null)
            {
                return NotFound();
            }
            return View(grad);
        }

        // POST: Grads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GradIme")] Grad grad)
        {
            if (id != grad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradExists(grad.Id))
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
            return View(grad);
        }

        // GET: Grads/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Grad == null)
            {
                return NotFound();
            }

            var grad = await _context.Grad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grad == null)
            {
                return NotFound();
            }

            return View(grad);
        }

        // POST: Grads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Grad == null)
            {
                return Problem("Entity set 'GradinataNaBabaRatkaContext.Grad'  is null.");
            }
            var grad = await _context.Grad.FindAsync(id);
            if (grad != null)
            {
                _context.Grad.Remove(grad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradExists(int id)
        {
          return (_context.Grad?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
