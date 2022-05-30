using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPProject1.Data;
using Microsoft.AspNetCore.Authorization;

namespace ASPProject1.Controllers
{
    public class RepertoiresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepertoiresController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Repertories.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repertoire == null)
            {
                return NotFound();
            }

            return View(repertoire);
        }
        [Authorize(Roles = "Admin")]       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Video,Data")] Repertoire repertoire)
        {
            Repertoire repertoire1 = new Repertoire()
            {
                Data = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                _context.Add(repertoire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            return View(repertoire);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertories.FindAsync(id);
            if (repertoire == null)
            {
                return NotFound();
            }
            return View(repertoire);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Video,Data")] Repertoire repertoire)
        {
            if (id != repertoire.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repertoire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepertoireExists(repertoire.Id))
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
            return View(repertoire);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var repertoire = await _context.Repertories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repertoire == null)
            {
                return NotFound();
            }
            return View(repertoire);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repertoire = await _context.Repertories.FindAsync(id);
            _context.Repertories.Remove(repertoire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool RepertoireExists(int id)
        {
            return _context.Repertories.Any(e => e.Id == id);
        }
    }
}
