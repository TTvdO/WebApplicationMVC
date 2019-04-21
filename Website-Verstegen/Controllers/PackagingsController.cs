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
    public class PackagingsController : Controller
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public PackagingsController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Packagings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Packagings.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> PackagingDetails(int? id)
        {
            var packaging = await _context.Packagings.FindAsync(id);
            return View(packaging);
        }

        // GET: Packagings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packaging = await _context.Packagings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (packaging == null)
            {
                return NotFound();
            }

            return View(packaging);
        }

        // GET: Packagings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Packagings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Category,Description,ImageUrl,AltText,Contents,PackagingWidth,PackagingHeight")] Packaging packaging, IFormFile ImageUrl)
        {
            //Makes the local url where the image is located
            string[] paths = { _hostingEnvironment.WebRootPath, "images", packaging.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            if (ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    if (ModelState.IsValid)
                    {
                        NewPackagingToDatabase(packaging, path);
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
                            NewPackagingToDatabase(packaging, path);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(packaging);
        }

        //Creates/Updates The Package To The Database
        private void NewPackagingToDatabase(Packaging packaging, string path)
        {
            packaging.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
            _context.Update(packaging);
            _context.Packagings.Add(packaging);
        }

        // GET: Packagings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packaging = await _context.Packagings.FindAsync(id);
            if (packaging == null)
            {
                return NotFound();
            }
            return View(packaging);
        }

        // POST: Packagings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Category,Description,ImageUrl,AltText,Contents,PackagingWidth,PackagingHeight")] Packaging packaging, IFormFile ImageUrl)
        {
            //Makes the local url where the image is located
            string[] paths = { _hostingEnvironment.WebRootPath, "images", packaging.Type + "s".ToLower() };
            var upload = Path.Combine(paths);
            
            if (ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    WriteToDatabase(packaging, path);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Actually makes the url from the image to an image
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                        WriteToDatabase(packaging, path);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Update(packaging);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(packaging);
        }

        //Updates Packaging In The Database
        private void WriteToDatabase(Packaging packaging, string path)
        {
            if (ModelState.IsValid)
            {
                packaging.ImageUrl = path.Substring(path.LastIndexOf("images"));
                _context.Update(packaging);
            }
        }

        // GET: Packagings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packaging = await _context.Packagings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (packaging == null)
            {
                return NotFound();
            }

            return View(packaging);
        }

        // POST: Packagings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var packaging = await _context.Packagings.FindAsync(id);
            _context.Packagings.Remove(packaging);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
