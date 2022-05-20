using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPProject1.Data;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using ASPProject1.Models;
using Microsoft.AspNetCore.Hosting;

namespace ASPProject1.Controllers
{
    public class TrophiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string wwwroot;

        public TrophiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trophies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trophys.ToListAsync());
        }

        // GET: Trophies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trophy = await _context.Trophys
                .Include(img => img.TrophyImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trophy == null)
            {
                return NotFound();
            }
            var imagePath1 = Path.Combine(wwwroot, "TrophyImages");
            TrophyDetailsVM modelVM = new TrophyDetailsVM()
            {
                Name = trophy.Name,
                Description = trophy.Description,
                Data = trophy.Data,

                ImagesPaths1 = _context.TrophyImages
                .Where(img => img.TrophyId == trophy.Id)
                .Select(x => $"/TrophyImages/{x.ImagePath1}").ToList<string>()
            };
            return View(modelVM);


        }

        // GET: Trophies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task CreateImages(TrophyVM model)
        {
            Trophy productToDb = new Trophy()
            {
                Name = model.Name,
                Description = model.Description,
                Data = DateTime.Now
            };
            await _context.Trophys.AddAsync(productToDb);
            await this._context.SaveChangesAsync();

            //var wwwroot = $"{this._hostEnvironment.WebRootPath}";
            //създаваме папката images, ако не съществува
            Directory.CreateDirectory($"{wwwroot}/TrophyImages/");
            var imagePath1 = Path.Combine(wwwroot, "TrophyImages");
            string uniqueFileName = null;
            if (model.ImagePath1.Count > 0)
            {
                for (int i = 0; i < model.ImagePath1.Count; i++)
                {
                    TrophyImages dbImage = new TrophyImages()
                    {
                        TrophyId = productToDb.Id,
                        Trophy = productToDb
                    };//id се създава автоматично при създаване на обект
                    if (model.ImagePath1[i] != null)
                    {
                        uniqueFileName = dbImage.Id + "_" + model.ImagePath1[i].FileName;
                        string filePath = Path.Combine(imagePath1, uniqueFileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImagePath1[i].CopyToAsync(fileStream);
                        }

                        dbImage.ImagePath1 = uniqueFileName;
                        await _context.TrophyImages.AddAsync(dbImage);
                        await this._context.SaveChangesAsync();
                    }
                }
            }
        }

        // POST: Trophies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Fotos,Data")] Trophy trophy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trophy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trophy);
        }

        // GET: Trophies/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trophy = await _context.Trophys.FindAsync(id);
            if (trophy == null)
            {
                return NotFound();
            }
            TrophyDetailsVM model = new TrophyDetailsVM
            {
                Id = trophy.Id,
                Name = trophy.Name,
                Description = trophy.Description,
                Data = trophy.Data,

                ImagesPaths1 = _context.TrophyImages
                .Where(img => img.TrophyId == trophy.Id)
                .Select(x => $"/TrophyImages/{x.ImagePath1}").ToList<string>()
            };

            return View(model);

        }

        // POST: Trophies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Fotos,Data")] Trophy trophy)
        {
            if (id != trophy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trophy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrophyExists(trophy.Id))
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
            return View(trophy);
        }

        // GET: Trophies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trophy = await _context.Trophys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trophy == null)
            {
                return NotFound();
            }

            return View(trophy);
        }

        // POST: Trophies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trophy = await _context.Trophys.FindAsync(id);
            _context.Trophys.Remove(trophy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrophyExists(int id)
        {
            return _context.Trophys.Any(e => e.Id == id);
        }
    }
}
