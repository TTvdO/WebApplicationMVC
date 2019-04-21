using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; } = "Product";

        [Required]
        public string Category { get; set; }
        
        public string ImageUrl { get; set; }

        [Required]
        public string Description { get; set; }

        public string Contents { get; set; }

        public string DownloadLink{ get; set; }
        
        public List<ProductDetails> ProductDetails { get; set; }

        public virtual List<ThemeProduct> ThemeProducts { get; set; }
    }
}
