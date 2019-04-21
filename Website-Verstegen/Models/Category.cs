using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Category
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public string Title { set; get; }
    }
}
