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
    public class CertificatesController : Controller
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public CertificatesController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Certificates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Certificates.ToListAsync());
        }

        // GET: Certificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // GET: Certificates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Alt_Text,Img_Src")] Certificate certificate, IFormFile Img_Src)
        {
            //Makes the local url where the image is located
            string[] pathArray = { _hostingEnvironment.WebRootPath, "images", "certificates" };
            var combinePath = Path.Combine(pathArray);

            //No Image Uploaded Means Back To The Create Action
            if (Img_Src != null)
            {
                var localPath = Path.Combine(combinePath, Img_Src.FileName);
                if (System.IO.File.Exists(localPath))
                {
                    if (ModelState.IsValid)
                    {
                        CertificateToDatabase(certificate, localPath);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    //Actually makes the url from the image to an image
                    using (var fileStream = new FileStream(localPath, FileMode.Create))
                    {
                        await Img_Src.CopyToAsync(fileStream);
                        if (ModelState.IsValid)
                        {
                            CertificateToDatabase(certificate, localPath);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(certificate);            
        }

        //Updates + Creates A New Certificate
        private void CertificateToDatabase(Certificate certificate, string localPath)
        {
            certificate.Img_Src = localPath.Substring(localPath.LastIndexOf("\\images"));
            _context.Update(certificate);
        }

        // GET: Certificates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }
            return View(certificate);
        }

        // POST: Certificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Alt_Text,Img_Src")] Certificate certificate, IFormFile Img_Src)
        {
            //Makes the local url where the image is located
            string[] pathArray = { _hostingEnvironment.WebRootPath, "images", "certificates" };
            var combinePath = Path.Combine(pathArray);

            //No Image Means This Action Takes The Current Image And Updates The Whole Certificate 
            if (Img_Src != null)
            {
                var localPath = Path.Combine(combinePath, Img_Src.FileName);
                if (System.IO.File.Exists(localPath))
                {
                    if (ModelState.IsValid)
                    {
                        CertificateToDatabase(certificate, localPath);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    //Actually makes the url from the image to an image
                    using (var fileStream = new FileStream(localPath, FileMode.Create))
                    {
                        await Img_Src.CopyToAsync(fileStream);
                        if (ModelState.IsValid)
                        {
                            CertificateToDatabase(certificate, localPath);
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
                    _context.Update(certificate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(certificate);
        }

        // GET: Certificates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
