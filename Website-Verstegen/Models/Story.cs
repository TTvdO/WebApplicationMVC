using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Website_Verstegen.Models
{
    public class Story
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; } = "Story";

        [Required]
        public string Category { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Please enter a Theme name.")]
        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }
    }
}