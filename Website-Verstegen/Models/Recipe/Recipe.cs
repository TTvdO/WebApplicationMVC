using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
        public string Type { get; set; } = "Recipe";

        public String Category { get; set; }
        public String ImageUrl { get; set; }

        public List<PreparationStep> PreparationSteps { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; }

    }
}
