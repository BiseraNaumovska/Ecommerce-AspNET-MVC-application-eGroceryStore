using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradinataNaBabaRatka.Data;
using GradinataNaBabaRatka.Models;
using GradinataNaBabaRatka.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GradinataNaBabaRatka.Controllers
{
    public class KupuvacsController : Controller
    {
        private readonly GradinataNaBabaRatkaContext _context;
        private readonly UserManager<GradinataNaBabaRatkaUser> _userManager;

        public KupuvacsController(GradinataNaBabaRatkaContext context, UserManager<GradinataNaBabaRatkaUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }
        private Task<GradinataNaBabaRatkaUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Kupuvacs
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var gradinataNaBabaRatkaContext = _context.Kupuvac.Include(k => k.Proizvod);
            return gradinataNaBabaRatkaContext != null ?
                          View(await gradinataNaBabaRatkaContext.ToListAsync()) :
                          Problem("Entity set 'GradinataNaBabaRatkaContext.Kupuvac'  is null.");
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddProizvodBought(int? bookid)
        {
            if (bookid == null)
            {
                return NotFound();
            }
            var gradinataNaBabaRatkaContext = _context.Kupuvac.Where(r => r.ProizvodId == bookid).Include(p => p.Proizvod).ThenInclude(p => p.Prodavac);
            var user = await GetCurrentUserAsync();

            if (ModelState.IsValid)
            {
                Kupuvac userbook = new Kupuvac();
                userbook.ProizvodId = (int)bookid;
                userbook.AppUser = user.UserName;
                _context.Kupuvac.Add(userbook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MojataListaNaProizvodit));
            }
            return gradinataNaBabaRatkaContext != null ?
                          View(await gradinataNaBabaRatkaContext.ToListAsync()) :
                          Problem("Entity set 'GradinataNaBabaRatkaContext.UserBook'  is null.");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MojataListaNaProizvodit()
        {
            var user = await GetCurrentUserAsync();
            var gradinataNaBabaRatkaContext = _context.Kupuvac.AsQueryable().Where(r => r.AppUser == user.UserName).Include(r => r.Proizvod).ThenInclude(p => p.Prodavac);
            var books_ofcurrentuser = _context.Proizvod.AsQueryable(); ;
            books_ofcurrentuser = gradinataNaBabaRatkaContext.Select(p => p.Proizvod);
            return gradinataNaBabaRatkaContext != null ?
                          View("~/Views/Kupuvacs/KupeniProizvodi.cshtml", await books_ofcurrentuser.ToListAsync()) :
                          Problem("Entity set 'GradinataNaBabaRatkaContext.Kupuvac'  is null.");
        }


        // GET: Kupuvacs/Details/5
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Kupuvac == null)
            {
                return NotFound();
            }

            var kupuvac = await _context.Kupuvac
                .Include(k => k.Proizvod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kupuvac == null)
            {
                return NotFound();
            }

            return View(kupuvac);
        }

        // GET: Kupuvacs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Kupuvacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,ProizvodId")] Kupuvac kupuvac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kupuvac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(kupuvac);
        }

        // GET: Kupuvacs/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Kupuvac == null)
            {
                return NotFound();
            }

            var kupuvac = await _context.Kupuvac.FindAsync(id);
            if (kupuvac == null)
            {
                return NotFound();
            }

            return View(kupuvac);
        }

        // POST: Kupuvacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUser,ProizvodId")] Kupuvac kupuvac)
        {
            if (id != kupuvac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kupuvac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KupuvacExists(kupuvac.Id))
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
            
            return View(kupuvac);
        }

        // GET: Kupuvacs/Delete/5
        [Authorize(Roles = "User")]

        public async Task<IActionResult> DeleteOwnedProizvod(int? bookid)
        {
            if (bookid == null || _context.Kupuvac == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var userBook = await _context.Kupuvac.Include(p => p.Proizvod).AsQueryable().FirstOrDefaultAsync(m => m.AppUser == user.UserName && m.ProizvodId == bookid);
            if (userBook == null)
            {
                return NotFound();
            }

            return View("~/Views/kupuvacs/Delete.cshtml", userBook);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Kupuvac == null)
            {
                return NotFound();
            }

            var kupuvac = await _context.Kupuvac
                .Include(k => k.Proizvod).AsQueryable()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kupuvac == null)
            {
                return NotFound();
            }

            return View(kupuvac);
        }

        // POST: Kupuvacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Kupuvac == null)
            {
                return Problem("Entity set 'GradinataNaBabaRatkaContext.Kupuvac'  is null.");
            }
            var kupuvac = await _context.Kupuvac.FindAsync(id);
            if (kupuvac != null)
            {
                _context.Kupuvac.Remove(kupuvac);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KupuvacExists(int id)
        {
          return (_context.Kupuvac?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
