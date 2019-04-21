using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewModels
{
    public class Recipe_IngredientViewModel
    {
        public IEnumerable<Recipe> Recipes { get; set; }
        public IEnumerable<Ingredient> IngredientsFirstRecipe { get; set; }
        public IEnumerable<Ingredient> IngredientsSecondRecipe { get; set; }
    }
}
