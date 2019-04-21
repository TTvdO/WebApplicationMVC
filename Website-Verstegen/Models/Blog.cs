using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Blog
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public string Title { set; get; }
        public string Subtitle { set; get; }
        [Required]
        public string Type { set; get; } = "Blog";
        [Required]
        public string Category { get; set; }
        [Required]
        public string Content { set; get; }
        public string ImageUrl { set; get; }
    }
}
