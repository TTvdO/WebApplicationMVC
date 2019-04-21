using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website_Verstegen.Data;
using Website_Verstegen.Models;

namespace Website_Verstegen.Controllers
{
    public class StoriesController : Controller
    {
        private readonly DatabaseContext _context;

        private IHostingEnvironment _hostingEnvironment;

        public StoriesController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Stories
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Stories.Include(s => s.Theme);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Stories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories
                .Include(s => s.Theme)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // GET: Stories/Create
        public IActionResult Create()
        {
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName");
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Type,Category,ImageUrl,ThemeId")] Story story, IFormFile ImageUrl)
        {
            if (_context.Stories.Any(s => s.Title == story.Title && s.Id != story.Id))
            {
                ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", story.ThemeId);
                ViewData["Error"] = "This story name already exists.";
                return View(story);
            }
            else if (ImageUrl == null)
            {
                ViewData["ImageNull"] = "Please select an image.";
            }
            else
            {
                string[] paths = { _hostingEnvironment.WebRootPath, "images", "stories" };
                var upload = Path.Combine(paths);

                if (ImageUrl != null)
                {
                    var path = Path.Combine(upload, ImageUrl.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        if (ModelState.IsValid)
                        {
                            story.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                            _context.Update(story);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        //Actually makes the url from the image to an image
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await ImageUrl.CopyToAsync(fileStream);
                            if (ModelState.IsValid)
                            {
                                story.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                                _context.Update(story);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", story.ThemeId);
            return View(story);
        }

        // GET: Stories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories.FindAsync(id);
            if (story == null)
            {
                return NotFound();
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", story.ThemeId);
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Type,Category,ImageUrl,ThemeId")] Story story, IFormFile ImageUrl)
        {
            if (id != story.Id)
            {
                return NotFound();
            }


            else if (_context.Stories.Any(s => s.Title == story.Title && s.Id != story.Id))
            {
                ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", story.ThemeId);
                ViewData["Error"] = "This story name already exists.";
                return View(story);
            }

            else
            {
                string[] paths = { _hostingEnvironment.WebRootPath, "images", "stories" };
                var upload = Path.Combine(paths);

                if (ImageUrl != null)
                {
                    var path = Path.Combine(upload, ImageUrl.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        story.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                        _context.Update(story);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //Actually makes the url from the image to an image
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            if (ModelState.IsValid)
                            {
                                await ImageUrl.CopyToAsync(fileStream);
                                story.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                                _context.Update(story);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }

                else
                {
                    if (ModelState.IsValid)
                    {
                        _context.Update(story);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", story.ThemeId);
            return View(story);
        }

        // GET: Stories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories
                .Include(s => s.Theme)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoryExists(int id)
        {
            return _context.Stories.Any(e => e.Id == id);
        }
    }
}