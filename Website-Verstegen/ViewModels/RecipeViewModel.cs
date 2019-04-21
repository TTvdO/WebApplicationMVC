using System.Collections.Generic;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewModels
{
    public class RecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<PreparationStep> PreparationSteps { get; set; }
        public IEnumerable<RecipeIngredient> RecipeIngredients { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
