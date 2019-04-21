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
    public class BlogsController : Controller
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public BlogsController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Title,Subtitle,Category,Content,ImageUrl")] Blog blog, IFormFile ImageUrl)
        {
            //Makes the local url where the image is located
            string[] paths = { _hostingEnvironment.WebRootPath, "images", blog.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            if (ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    if (ModelState.IsValid)
                    {
                        NewBlogToDatabase(blog, path);
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
                            NewBlogToDatabase(blog, path);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Title,Subtitle,Category,Content,ImageUrl")] Blog blog, IFormFile ImageUrl)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            string[] paths = { _hostingEnvironment.WebRootPath, "images", blog.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            if (ImageUrl != null)
            {
                
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    WriteToDatabase(blog, path);
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                        WriteToDatabase(blog, path);
                        _context.Update(blog);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(blog);
        }

        //Updates Database With A New Image Path
        private void WriteToDatabase(Blog blog, string path)
        {
            if (ModelState.IsValid)
            {
                blog.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                _context.Update(blog);
            }
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }

        //Writes A New Blog to The Database
        private void NewBlogToDatabase(Blog blog, string path)
        {
            blog.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
            _context.Update(blog);
        }
    }
}
