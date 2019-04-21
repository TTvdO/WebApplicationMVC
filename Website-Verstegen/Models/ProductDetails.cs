using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class ProductDetails
    {
        [Key]
        public int Id { get; set; }
        public string Details { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
