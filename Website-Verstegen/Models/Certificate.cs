using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Alt_Text { get; set; }
        
        public string Img_Src { get; set; }
    }
}
