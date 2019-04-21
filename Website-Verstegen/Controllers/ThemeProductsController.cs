using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.ViewModels;

namespace Website_Verstegen.Controllers
{
    public class ThemeProductsController : Controller
    {
        private readonly DatabaseContext _context;

        public ThemeProductsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: ThemeProducts
        public async Task<IActionResult> Index()
        {
            //Get all the ThemeProducts and include the relationships of the product and theme.
            var context = _context.ThemeProducts.Include(t => t.Product).Include(t => t.Theme);
            return View(await context.ToListAsync());
        }

        // GET: ThemeProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the first ThemeProduct with the same ThemeProductId as the id that has been clicked on.
            var themeProduct = await _context.ThemeProducts
                .Include(t => t.Product)
                .Include(t => t.Theme)
                .FirstOrDefaultAsync(m => m.ThemeProductId == id);
            if (themeProduct == null)
            {
                return NotFound();
            }

            return View(themeProduct);
        }

        // GET: ThemeProducts/Create
        public IActionResult Create()
        {
            //Two drop-down lists to select which Product and Theme you want to link to each other.
            ViewData["Id"] = new SelectList(_context.Products, "Id", "Title");
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName");
            return View();
        }

        // POST: ThemeProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThemeProductId,Id,ThemeId")] ThemeProduct themeProduct)
        {
            if (ModelState.IsValid)
            {
                //If there is already a ThemeProduct with the same Theme and Product relationship and with a different ThemeProductId than the one you are creating, don't Create it and show
                //an error.
                if (_context.ThemeProducts.Any(tp => tp.Product.Id == themeProduct.Id && tp.ThemeId == themeProduct.ThemeId && tp.ThemeProductId != themeProduct.ThemeProductId))
                {
                    ViewData["Error"] = "This Product and Theme combination already exists.";
                    ViewData["Id"] = new SelectList(_context.Products, "Id", "Title", themeProduct.Id);
                    ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", themeProduct.ThemeId);
                    return View(themeProduct);
                }
                //If it's a unique relationship between the Theme and Product, create it.
                else
                {
                    _context.Add(themeProduct);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            //If ModelState isn't valid, don't create it.
            ViewData["Id"] = new SelectList(_context.Products, "Id", "Title", themeProduct.Id);
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", themeProduct.ThemeId);
            return View(themeProduct);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var currentThemeProduct = await _context.ThemeProducts.FindAsync(id);
            List<Product> allProducts = _context.Products.ToList();
            List<Theme> allThemes = _context.Themes.ToList();

            ThemeProducts_ViewModel themeProduct = new ThemeProducts_ViewModel { CurrentThemeProduct = currentThemeProduct, Products = allProducts, Themes = allThemes};

            ViewData["Products"] = new SelectList(_context.Products, "Id", "Title", currentThemeProduct.Product.Id);
            ViewData["Themes"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", currentThemeProduct.Theme.ThemeId);
            return View(themeProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThemeProductId,Id,ThemeId")] ThemeProduct themeProduct)
        {
            var themeProductToUpdate = _context.ThemeProducts.Where(tp => tp.ThemeProductId == id).Single();
            var productId = Convert.ToInt32(Request.Form["productId"]);
            var themeId = Convert.ToInt32(Request.Form["themeId"]);

            themeProductToUpdate.Id = productId;
            themeProductToUpdate.ThemeId = themeId;
            if (ModelState.IsValid)
            {
                _context.Update(themeProductToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(themeProduct);
        }

        // GET: ThemeProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the first ThemeProduct with the same ThemeProductId as the id that has been clicked on.
            var themeProduct = await _context.ThemeProducts
                .Include(t => t.Product)
                .Include(t => t.Theme)
                .FirstOrDefaultAsync(m => m.ThemeProductId == id);
            if (themeProduct == null)
            {
                return NotFound();
            }

            return View(themeProduct);
        }

        // POST: ThemeProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Get the first ThemeProduct with the same ThemeProductId as the id that has been clicked on.
            var themeProduct = await _context.ThemeProducts.FindAsync(id);
            _context.ThemeProducts.Remove(themeProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThemeProductExists(int id)
        {
            return _context.ThemeProducts.Any(e => e.ThemeProductId == id);
        }
    }
}
