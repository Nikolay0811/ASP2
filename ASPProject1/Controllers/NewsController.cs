using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPProject1.Data;
using System.IO;
using ASPProject1.Models;
using Microsoft.AspNetCore.Hosting;

namespace ASPProject1.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string wwwroot;

        public NewsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwroot = $"{this._hostEnvironment.WebRootPath}";
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            return View(await _context.Newes.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            News product = await _context.Newes
                .Include(img => img.NewsImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(wwwroot, "NewsImages");
            NewsDetailsVM modelVM = new NewsDetailsVM()
            {
                Name = product.Name, 
                Text = product.Text,
                Data= product.Data,
               
                ImagesPaths = _context.NewsImages
                .Where(img => img.NewsId == product.Id)
                .Select(x => $"/NewsImages/{x.ImagePath}").ToList<string>()
            };
            return View(modelVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewsVM news)
        {
            {
                if (!ModelState.IsValid)
                {
                    return View(news);
                }

                await this.CreateImages(news);

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //1. зареждам искания id от БД .... за промяна на стойностите
            var product = await _context.Newes.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            //2. Създавм модела, с който ще визуализирам за промяна на стойностите
            //3. Пълня от БД в полетата на екрана
            NewsDetailsVM model = new NewsDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Text = product.Text,
                Data = product.Data,

                ImagesPaths = _context.NewsImages
                .Where(img => img.NewsId == product.Id)
                .Select(x => $"/NewsImages/{x.ImagePath}").ToList<string>()
            };

            return View(model);
        }
       
      
        public async Task CreateImages(NewsVM model)
        {
            News productToDb = new News()
            {
                Name = model.Name,
                Text = model.Text,
                Data=model.Data,
                 
            }; 
            await _context.Newes.AddAsync(productToDb);
            await this._context.SaveChangesAsync();

            //var wwwroot = $"{this._hostEnvironment.WebRootPath}";
            //създаваме папката images, ако не съществува
            Directory.CreateDirectory($"{wwwroot}/NewsImages/");
            var imagePath = Path.Combine(wwwroot, "NewsImages");
            string uniqueFileName = null;
            if (model.ImagePath.Count > 0)
            {
                for (int i = 0; i < model.ImagePath.Count; i++)
                {
                    NewsImages dbImage = new NewsImages()
                    {
                        NewsId = productToDb.Id,
                        News = productToDb
                    };//id се създава автоматично при създаване на обект
                    if (model.ImagePath[i] != null)
                    {
                        uniqueFileName = dbImage.Id + "_" + model.ImagePath[i].FileName;
                        string filePath = Path.Combine(imagePath, uniqueFileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImagePath[i].CopyToAsync(fileStream);
                        }

                        dbImage.ImagePath = uniqueFileName;
                        await _context.NewsImages.AddAsync(dbImage);
                        await this._context.SaveChangesAsync();
                    }
                }
            }
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Text,Fotos,Data")] News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
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
            return View(news);
        }
      

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.Newes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.Newes.FindAsync(id);
            _context.Newes.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.Newes.Any(e => e.Id == id);
        }
    }
}
