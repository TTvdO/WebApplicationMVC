using System;
using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class PreparationStep
    {
        [Key]
        public int PreparationStepId { get; set; }
        public String Text { get; set; }
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}
