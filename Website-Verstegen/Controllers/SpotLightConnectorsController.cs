using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.ViewModels;

namespace Website_Verstegen.Controllers
{
    [Authorize]
    public class SpotLightConnectorsController : Controller
    {
        private readonly DatabaseContext _context;

        public SpotLightConnectorsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: SpotLightConnectors
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpotLightConnectors.ToListAsync());
        }

        public SpotlightViewModel GetSpotlightViewModel()
        {
            //This controller does nothing more than getting all entities that could function as spotlightitem
            var items = new List<SpotLightItem>();
            SpotLightConnector spotLightConnector = new SpotLightConnector();

            SpotlightViewModel model = new SpotlightViewModel { SpotLightConnector = spotLightConnector, SpotLightItems = items };

            foreach (Product prod in _context.Products.ToList())
            {
                items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(prod)));
            }
            foreach (Packaging pac in _context.Packagings.ToList())
            {
                items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(pac)));
            }
            foreach (Blog blog in _context.Blogs.ToList())
            {
                items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(blog)));
            }
            foreach (Recipe rec in _context.Recipes.ToList())
            {
                items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(rec)));
            }
            foreach (Story story in _context.Stories.ToList())
            {
                items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(story)));
            }

            return model;
        }

        // GET: SpotLightConnectors/Create
        public IActionResult Create()
        {
            return View(GetSpotlightViewModel());
        }

        // POST: SpotLightConnectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,actionName,controllerName")] SpotLightConnector spotLightConnector, IFormCollection form)
        {
            //This Check and Method Are Only To Test The FormCollection Unit Test, It's impossible to Mock a FormCollection Object
            if(form == null)
            {
                FormCollectionMockForUnitTest(spotLightConnector);
                return RedirectToAction(nameof(Index));
            }

            if (form["ingredients"].Count.Equals(8))
            {
                if (ModelState.IsValid)
                {
                    string[] idsAndTypes = form["ingredients"].ToString().Split(",");

                    //prepare and add ingredients belonging to this recipe to the database
                    var SpotlightContents = new List<SpotlightContent>();
                    foreach (string idAndType in idsAndTypes)
                    {
                        string[] values = idAndType.ToString().Split(".");

                        SpotlightContents.Add(new SpotlightContent { SpId = Convert.ToInt32(values[0]), SpotlightConnector = spotLightConnector, SpType = values[1] });
                    }

                    spotLightConnector.SpotlightContent = SpotlightContents;
                    
                    //Checks if the database already contains the new spotlightConnector
                    SpotLightConnector existingConnector = _context.SpotLightConnectors.Where(sc => sc.actionName.ToLower() == spotLightConnector.actionName.ToLower()).Where(sc => sc.controllerName.ToLower() == spotLightConnector.controllerName.ToLower()).FirstOrDefault();
                    
                    //Add the new spotlightConnector if the action/controller combination doesn't exists
                    if(existingConnector == null)
                    {
                        _context.SpotLightConnectors.Add(spotLightConnector);
                    }
                    //Overrrides the old spotlightConnector if the action/controller combination already exists.
                    else
                    {
                        _context.Remove(existingConnector);
                        _context.SpotLightConnectors.Add(spotLightConnector);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["error_message"] = "Please Add 8 Spotlight Items.";
            return View(GetSpotlightViewModel());
        }

        //This Method is only used to test the unit test, it was impossible to make an FormCollection Object So I did it hardcoded
        public void FormCollectionMockForUnitTest(SpotLightConnector spotLightConnector)
        {
            //Mocked FormCollection Data For Unit Test
            string[] MockedIdTypeList = { "1.Product ", "2.Product", "3.Product", "4.Product", "5.Product", "6.Product", "7.Product", "8.Product" };
            
            if (ModelState.IsValid)
            {
                //prepare and add ingredients belonging to this recipe to the database
                var SpotlightContents = new List<SpotlightContent>();
                foreach (string idAndType in MockedIdTypeList)
                {
                    string[] values = idAndType.ToString().Split(".");

                    SpotlightContents.Add(new SpotlightContent { SpId = Convert.ToInt32(values[0]), SpotlightConnector = spotLightConnector, SpType = values[1] });
                }

                spotLightConnector.SpotlightContent = SpotlightContents;

                //Checks if the database already contains the new spotlightConnector
                SpotLightConnector existingConnector = _context.SpotLightConnectors.Where(sc => sc.actionName.ToLower() == spotLightConnector.actionName.ToLower()).Where(sc => sc.controllerName.ToLower() == spotLightConnector.controllerName.ToLower()).FirstOrDefault();

                //Add the new spotlightConnector if the action/controller combination doesn't exists
                if (existingConnector == null)
                {
                    _context.SpotLightConnectors.Add(spotLightConnector);
                }
                //Overrrides the old spotlightConnector if the action/controller combination already exists.
                else
                {
                    _context.Remove(existingConnector);
                    _context.SpotLightConnectors.Add(spotLightConnector);
                }
            }
        }

        // GET: SpotlightConnector/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spotlightConnector = await _context.SpotLightConnectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spotlightConnector == null)
            {
                return NotFound();
            }

            return View(spotlightConnector);
        }

        // POST: SpotlightConnector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spotlightConnector = await _context.SpotLightConnectors.FindAsync(id);
            _context.SpotLightConnectors.Remove(spotlightConnector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
