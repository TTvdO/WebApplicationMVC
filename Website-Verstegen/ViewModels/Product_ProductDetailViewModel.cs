using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewModels
{
    public class Product_ProductDetailViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<ProductDetails> AllDetails { get; set; }
    }
}
