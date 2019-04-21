using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class RecipeIngredient
    {
        [Key]
        public int RecipeIngredientId { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
