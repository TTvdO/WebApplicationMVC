using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Verstegen.Data;
using Website_Verstegen.Models;

namespace Website_Verstegen.Controllers
{
    public class ThemesController : Controller
    {
        private readonly DatabaseContext _context;

        private IHostingEnvironment _hostingEnvironment;

        public ThemesController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Themes
        public async Task<IActionResult> Index(string sortOrder, string filter, string currentFilter, int? page)
        {
            if (String.IsNullOrEmpty(filter))
            {
                filter = currentFilter;
            }
            else
            {
                page = 1;
            }
            IQueryable<Theme> list = Sort(sortOrder);
            list = Filter(list, filter);
            return View(await Paging(list.Include(t => t.ThemeProducts).ThenInclude(tp => tp.Product), page));
        }

        // GET: Themes. For testing
        public async Task<IActionResult> TestIndex()
        {
            //Get the themes from the database and include the ThemeProducts for every Theme and every Product for every ThemeProduct. Including ThemeProducts and Products is for Unit Testing.
            //In the Unit Tests I make sure every Theme has the correct ThemeProducts and Product relationships.
            return View(await _context.Themes.Include(t => t.ThemeProducts).ThenInclude(tp => tp.Product).ToListAsync());
        }

        // GET: Themes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the first theme with the same ThemeId as the id that has been clicked on.
            var theme = await _context.Themes
                .FirstOrDefaultAsync(m => m.ThemeId == id);
            if (theme == null)
            {
                return NotFound();
            }

            return View(theme);
        }

        // GET: Themes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThemeId,ThemeName,Description")] Theme theme, IFormFile ImageUrl)
        {
            //Don't let a theme be created if there is already a different theme with the same name
            if (_context.Themes.Any(t => t.ThemeName == theme.ThemeName && t.ThemeId != theme.ThemeId))
            {
                ViewData["Error"] = "This theme name already exists.";
                return View(theme);
            }
            else if(ImageUrl == null)
            {
                ViewData["ImageNull"] = "Please select an image.";
            }
            else
            {
                string[] paths = { _hostingEnvironment.WebRootPath, "images", "themes" };
                var upload = Path.Combine(paths);

                if (ImageUrl != null)
                {
                    var path = Path.Combine(upload, ImageUrl.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        if (ModelState.IsValid)
                        {
                            //ImageUrl is now \images\(insert file name). Double backslashes are put as only one backslash when it is translated to a theme.ImageUrl string.
                            theme.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                            _context.Update(theme);
                            _context.Themes.Add(theme);
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
                                //ImageUrl is now \images\(insert file name). Double backslashes are put as only one backslash when it is translated to a theme.ImageUrl string.
                                theme.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                                _context.Update(theme);
                                _context.Themes.Add(theme);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            return View(theme);
        }

        // GET: Themes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the first theme with the same ThemeId as the id that has been clicked on.
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }
            return View(theme);
        }

        // POST: Themes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThemeId,ThemeName,Description,ImageUrl")] Theme theme, IFormFile ImageUrl)
        {
            if (id != theme.ThemeId)
            {
                return NotFound();
            }


            //Don't let a theme be created if there is already a different theme with the same name
            if (_context.Themes.Any(t => t.ThemeName == theme.ThemeName && t.ThemeId != theme.ThemeId))
            {
                ViewData["Error"] = "This theme name already exists.";
                return View(theme);
            }
            else
            {
                //Location of the folder to which the image will be added eventually, in an array of strings
                string[] paths = { _hostingEnvironment.WebRootPath, "images", "themes" };
                //Turns the strings into a path with backslashes behind each position of the string array
                var upload = Path.Combine(paths);

                if (ImageUrl != null)
                {
                    //Folder and image name together
                    var path = Path.Combine(upload, ImageUrl.FileName);
                    //If the ImageUrl name already exists in the destination folder
                    if (System.IO.File.Exists(path))
                    {
                        //ImageUrl is now \images\(insert file name). Double backslashes are put as only one backslash when it is translated to a theme.ImageUrl string.

                        theme.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                        _context.Update(theme);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //Actually makes the url from the image to an image
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await ImageUrl.CopyToAsync(fileStream);
                            if (ModelState.IsValid)
                            {
                                //ImageUrl is now \images\(insert file name). Double backslashes are put as only one backslash when it is translated to a theme.ImageUrl string.
                                theme.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                                _context.Update(theme);
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
                        _context.Update(theme);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(theme);
        }

        // GET: Themes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the first theme with the same ThemeId as the id that has been clicked on.
            var theme = await _context.Themes
                .FirstOrDefaultAsync(m => m.ThemeId == id);
            if (theme == null)
            {
                return NotFound();
            }

            return View(theme);
        }

        // POST: Themes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Find the theme that has the same id as the id that has been clicked on and is given as a parameter.
            var theme = await _context.Themes.FindAsync(id);
            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThemeExists(int id)
        {
            return _context.Themes.Any(e => e.ThemeId == id);
        }

        public IQueryable<Theme> Sort(string sortOrder)
        {
            ViewData["CurrentSort"] = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            ViewData["NewSort"] = sortOrder;

            var list = GetList();

            switch (sortOrder)
            {
                case "desc":
                    list = list.OrderByDescending(t => t.ThemeName);
                    break;
                default:
                    list = list.OrderBy(t => t.ThemeName);
                    break;
            }

            return list;
        }

        public IQueryable<Theme> Filter(IQueryable<Theme> list, string filter)
        {
            ViewData["CurrentFilter"] = filter;

            if (!String.IsNullOrEmpty(filter))
            {
                list = list.Where(t => t.ThemeName.ToLower().Contains(filter.ToLower()));
            }

            return list;
        }

        public async Task<Data.PaginatedList<Theme>> Paging(IQueryable<Theme> list, int? page)
        {
            page = page ?? 1;
            ViewData["CurrentPage"] = page;
            int amountOfItemsPerPage = 5;
            return await Data.PaginatedList<Theme>.CreateAsync(list.AsNoTracking(), (int)page, amountOfItemsPerPage);
        }

        public IQueryable<Theme> GetList()
        {
            return _context.Themes;
        }
    }
}
