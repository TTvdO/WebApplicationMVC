using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.ViewModels;

namespace Website_Verstegen.ViewComponents
{
    public class RecipeDoorwayViewComponent :ViewComponent
    {
        private readonly DatabaseContext _context;

        public RecipeDoorwayViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //Values for the ViewModel
            var recipes = _context.Recipes.Take(2).ToList();

            //Gets All The RecipeIngredients That Belong To The 2 Recipes
            var recipeFirstIngredients = _context.RecipeIngredients.Where(ri => ri.RecipeId == recipes.First().Id).ToList();
            var recipeSecondIngredients = _context.RecipeIngredients.Where(ri => ri.RecipeId == recipes.Last().Id).ToList();

            var ingredientsFirstRecipe = new List<Ingredient>();

            //Get All The Ingredients That Belong To The Recipe
            foreach (var item in recipeFirstIngredients)
            {
                ingredientsFirstRecipe.Add(_context.Ingredients.Where(i => i.Id == item.IngredientId).SingleOrDefault());
            }

            var ingredientsSecondRecipe = new List<Ingredient>();

            //Get All The Ingredients That Belong To The Recipe
            foreach (var item in recipeSecondIngredients)
            {
                ingredientsSecondRecipe.Add(_context.Ingredients.Where(i => i.Id == item.IngredientId).SingleOrDefault());
            }

            Recipe_IngredientViewModel recipe_IngredientViewModel = new Recipe_IngredientViewModel {
                Recipes = recipes,
                IngredientsFirstRecipe = ingredientsFirstRecipe,
                IngredientsSecondRecipe = ingredientsSecondRecipe
            };

            return View("/Views/Shared/_RecipeDoorwayPartial.cshtml", recipe_IngredientViewModel);
        }
    }
}
