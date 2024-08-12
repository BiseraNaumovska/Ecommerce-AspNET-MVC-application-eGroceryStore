using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradinataNaBabaRatka.Data;
using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Identity;
using GradinataNaBabaRatka.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GradinataNaBabaRatka.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly GradinataNaBabaRatkaContext _context;
        private readonly UserManager<GradinataNaBabaRatkaUser> _userManager;

        public ReviewsController(GradinataNaBabaRatkaContext context, UserManager<GradinataNaBabaRatkaUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        private Task<GradinataNaBabaRatkaUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var gradinataNaBabaRatkaContext = _context.Review.Include(r => r.Proizvod);
            return View(await gradinataNaBabaRatkaContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Proizvod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        public async Task<IActionResult> WriteReviewFromUserAsync(int? proizvodId)
        {
            ViewData["ProizvodId"] = proizvodId;
            var proizvod = _context.Proizvod.AsQueryable().Where(p => p.Id == proizvodId).FirstOrDefault();
            ViewData["ProizvodIme"] = proizvod.Ime;
            return View();
        }

        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WriteReviewFromUser([Bind("Id,AppUser,Comment,Rating,ProizvodId")] Review review)
        {
            GradinataNaBabaRatkaUser user = await GetCurrentUserAsync();
            review.AppUser = user.UserName;
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProizvodId"] = review.ProizvodId;
            return View(review);
        }



        // GET: Reviews/Create
        [Authorize(Roles = "")]
        public IActionResult Create()
        {
            ViewData["ProizvodId"] = new SelectList(_context.Proizvod, "Id", "Ime");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,Comment,Rating,ProizvodId")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProizvodId"] = new SelectList(_context.Proizvod, "Id", "Opis", review.ProizvodId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize(Roles = "")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["ProizvodId"] = new SelectList(_context.Proizvod, "Id", "Ime", review.ProizvodId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUser,Comment,Rating,ProizvodId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["ProizvodId"] = new SelectList(_context.Proizvod, "Id", "Ime", review.ProizvodId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Proizvod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Review == null)
            {
                return Problem("Entity set 'GradinataNaBabaRatkaContext.Review'  is null.");
            }
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                _context.Review.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
          return (_context.Review?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
