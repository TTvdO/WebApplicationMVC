using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Packaging
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Type { get; set; } = "Packaging";
        [Required]
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string AltText { get; set; }

        [Required]
        public string Contents { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int PackagingWidth { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int PackagingHeight { get; set; }
    }
}
