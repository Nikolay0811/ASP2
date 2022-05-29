﻿using System;
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

        public TrophiesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwroot = $"{this._hostEnvironment.WebRootPath}";
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trophys.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Trophy trophy = await _context.Trophys
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

                ImagesPaths = _context.TrophyImages
                .Where(img => img.TrophyId == trophy.Id)
                .Select(x => $"/TrophyImages/{x.ImagePath}").ToList<string>()
            };
            return View(modelVM);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task CreateImages(TrophyVM model)
        {
            Trophy trophyToDb = new Trophy()
            {
                Name = model.Name,
                Description = model.Description,
                Data = DateTime.Now
            };
            await _context.Trophys.AddAsync(trophyToDb);
            await this._context.SaveChangesAsync();
            Directory.CreateDirectory($"{wwwroot}/TrophyImages/");
            var imagePath1 = Path.Combine(wwwroot, "TrophyImages");
            string uniqueFileName = null;
            if (model.ImagePath.Count > 0)
            {
                for (int i = 0; i < model.ImagePath.Count; i++)
                {
                    TrophyImages dbImage = new TrophyImages()
                    {
                        TrophyId = trophyToDb.Id,
                        Trophy = trophyToDb
                    };//id се създава автоматично при създаване на обект
                    if (model.ImagePath[i] != null)
                    {
                        uniqueFileName = dbImage.Id + "_" + model.ImagePath[i].FileName;
                        string filePath = Path.Combine(imagePath1, uniqueFileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImagePath[i].CopyToAsync(fileStream);
                        }

                        dbImage.ImagePath = uniqueFileName;
                        await _context.TrophyImages.AddAsync(dbImage);
                        await this._context.SaveChangesAsync();
                    }
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] TrophyVM trophy)
        {
            if (!ModelState.IsValid)
            {
               return View(trophy);
            } 
            await this.CreateImages(trophy);

            return RedirectToAction(nameof(Index));
            
        }
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

                ImagesPaths = _context.TrophyImages
                .Where(img => img.TrophyId == trophy.Id)
                .Select(x => $"/TrophyImages/{x.ImagePath}").ToList<string>()
            };
            return View(model);
        }
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
