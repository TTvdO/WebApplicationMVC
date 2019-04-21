using System;
using System.Collections.Generic;
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
using Website_Verstegen.ViewModels;

namespace Website_Verstegen.Controllers
{
    public class RecipesController : Controller
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public RecipesController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipes.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> RecipeDetails(int? id)
        {
            //Values for the ViewModel
            var Recipe = await _context.Recipes.FindAsync(id);
            List<PreparationStep> PreparationSteps = _context.PreparationStep.Where(ps => ps.RecipeId == id).ToList();
            List<RecipeIngredient> RecipeIngredients = _context.RecipeIngredients.Where(ri => ri.RecipeId == id).ToList();

            RecipeViewModel model = new RecipeViewModel { Recipe = Recipe, PreparationSteps = PreparationSteps, RecipeIngredients = RecipeIngredients };

            if (id == null) return NotFound();        

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Overview(string itemtypes, string sortOrder, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewData["DatumSortParm"] = sortOrder == "Datum" ? "Datum_desc" : "Datum";

            var recipes = from r in _context.Recipes select r;

            if (itemtypes != null)
            {
                recipes = _context.Recipes.Where(r => r.Category == itemtypes);
            }

            switch (sortOrder)
            {
                case "Title_desc":
                    recipes = recipes.OrderByDescending(s => s.Title);
                    break;
                case "Category":
                    recipes = recipes.OrderBy(s => s.Category);
                    break;
                case "Category_desc":
                    recipes = recipes.OrderByDescending(s => s.Category);
                    break;
                case "Datum":
                    recipes = recipes.OrderBy(s => s.Id);
                    break;
                case "Datum_desc":
                    recipes = recipes.OrderByDescending(s => s.Id);
                    break;
                default:
                    recipes = recipes.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Recipe>.CreateAsync(recipes.AsNoTracking(), page ?? 1, pageSize));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Overview(IFormCollection form, string sortOrder, int? page)
        {
            var itemValues = form["itemtypes"];

            var recipes = from r in _context.Recipes where r.Category == itemValues select r;

            int pageSize = 6;
            return View(await PaginatedList<Recipe>.CreateAsync(recipes.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            List<Ingredient> ingredients = _context.Ingredients.ToList();

            RecipeViewModel model = new RecipeViewModel { Ingredients = ingredients };

            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,ImageUrl")] Recipe recipe, IFormFile ImageUrl, IFormCollection form)
        {
            //Makes the local url where the image is located
            string[] paths = {_hostingEnvironment.WebRootPath, "images", recipe.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            if(ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    if (ModelState.IsValid)
                    {
                        NewRecipeToDatabase(recipe, path, form);
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
                            NewRecipeToDatabase(recipe, path, form);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(recipe);
        }

        //Writes A New Product to The Database
        private void NewRecipeToDatabase(Recipe recipe, string path, IFormCollection form)
        {
            recipe.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
            _context.Update(recipe);
            AddPreparationSteps(recipe, form);
            AddIngredients(recipe, form);
        }

        //Adds all the product details 
        private void AddPreparationSteps(Recipe recipe, IFormCollection form)
        {
            var PreparationSteps = new List<PreparationStep>();
            for (int i = 1; i <= Convert.ToInt32(form["product_details_counter"]); i++)
            {
                PreparationSteps.Add(new PreparationStep { Recipe = recipe, RecipeId = recipe.Id, Text = Request.Form["product_detail_" + i] });
            }
            PreparationSteps.ForEach(dp => _context.PreparationStep.Add(dp));
        }

        private void AddIngredients(Recipe recipe, IFormCollection form)
        {
            string[] RecipesIds = form["ingredients"].ToString().Split(",");

            if (RecipesIds.Contains(""))
            {
                //RecipeIds is submitted by unit test and empty. This hack is kinda dirty but ingredients form cannot be exactly mocked on ingredients because lack of proper documentation
            }

            else
            {
                //prepare and add ingredients belonging to this recipe to the database
                var RecipeIngredients = new List<RecipeIngredient>();
                foreach (string RecipeId in RecipesIds)
                {
                    RecipeIngredients.Add(new RecipeIngredient { Recipe = recipe, RecipeId = recipe.Id, IngredientId = Convert.ToInt32(RecipeId) });
                }
                _context.RecipeIngredients.AddRange(RecipeIngredients);
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var Recipe = await _context.Recipes.FindAsync(id);
            List<PreparationStep> PreparationSteps = _context.PreparationStep.Where(ps => ps.RecipeId == id).ToList();
            List<RecipeIngredient> RecipeIngredients = _context.RecipeIngredients.Where(ri => ri.RecipeId == id).ToList();
            List<Ingredient> Ingredients = new List<Ingredient>();
            foreach (RecipeIngredient ri in RecipeIngredients)
            {
                Ingredients.AddRange(_context.Ingredients.Where(i => i.Id == ri.IngredientId).ToList());
            }

            RecipeViewModel model = new RecipeViewModel { Recipe = Recipe, PreparationSteps = PreparationSteps, RecipeIngredients = RecipeIngredients, Ingredients = Ingredients };

            if (id == null)
            {
                return NotFound();
            }
            
            if (Recipe == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,ImageUrl")] Recipe recipe, IFormFile ImageUrl, IFormCollection form)
        {
            //Gets all the product details from the View
            var formValues = form["product_detail"];

            if (id != recipe.Id)
            {
                return NotFound();
            }

            string[] paths = { _hostingEnvironment.WebRootPath, "images", recipe.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            if(ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    WriteToDatabase(recipe, path);
                    UpdateProductDetails(formValues, recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                        WriteToDatabase(recipe, path);
                        UpdateProductDetails(formValues, recipe);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    UpdateProductDetails(formValues, recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(recipe);
        }

        //Updates The Product Details And Product
        private void UpdateProductDetails(string[] newDetails, Recipe recipe)
        {
            List<PreparationStep> currentPreparationSteps = _context.PreparationStep.Where(p => p.RecipeId == recipe.Id).ToList();
            if (ModelState.IsValid)
            {
                for (int i = 0; i < currentPreparationSteps.Count(); i++)
                {

                    if (newDetails.Any())
                    {
                        currentPreparationSteps[i].Text = newDetails[i];
                    }
                }
                _context.Update(recipe);
            }
        }

        //Updates Database With A New Image Path
        private void WriteToDatabase(Recipe recipe, string path)
        {
            if (ModelState.IsValid)
            {
                recipe.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                _context.Update(recipe);
            }
        }

        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
