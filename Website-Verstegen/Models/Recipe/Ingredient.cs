using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Unit { get; set; }
        public int Amount { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
