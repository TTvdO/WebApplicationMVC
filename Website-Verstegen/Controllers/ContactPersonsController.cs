using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website_Verstegen.Data;
using Website_Verstegen.Models;

namespace Website_Verstegen.Controllers
{
    public class ContactPersonsController : Controller
    {
        private readonly DatabaseContext _context;

        public ContactPersonsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: ContactPersons
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.People.Include(c => c.Country).Include(c => c.Province);
            return View(await databaseContext.ToListAsync());
        }

        // GET: ContactPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactPerson = await _context.People
                .Include(c => c.Country)
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactPerson == null)
            {
                return NotFound();
            }

            return View(contactPerson);
        }

        // GET: ContactPersons/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Naam");
            ViewData["ProvinceId"] = new SelectList(_context.Counties, "Id", "Naam");
            return View();
        }

        // POST: ContactPersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Functie,Telefoonnummer,EmailAdres,ImageUrl,CountryId,ProvinceId")] ContactPerson contactPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", contactPerson.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.Counties, "Id", "Id", contactPerson.ProvinceId);
            return View(contactPerson);
        }

        // GET: ContactPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactPerson = await _context.People.FindAsync(id);
            if (contactPerson == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", contactPerson.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.Counties, "Id", "Id", contactPerson.ProvinceId);
            return View(contactPerson);
        }

        // POST: ContactPersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Functie,Telefoonnummer,EmailAdres,ImageUrl,CountryId,ProvinceId")] ContactPerson contactPerson)
        {
            if (id != contactPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactPersonExists(contactPerson.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", contactPerson.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.Counties, "Id", "Id", contactPerson.ProvinceId);
            return View(contactPerson);
        }

        // GET: ContactPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactPerson = await _context.People
                .Include(c => c.Country)
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactPerson == null)
            {
                return NotFound();
            }

            return View(contactPerson);
        }

        // POST: ContactPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactPerson = await _context.People.FindAsync(id);
            _context.People.Remove(contactPerson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactPersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
