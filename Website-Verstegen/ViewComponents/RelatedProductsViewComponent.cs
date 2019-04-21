using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public RelatedProductsViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(Product product)
        {
            var products = _context.Products.Where(p => p.Category == product.Category && p.Id != product.Id).Take(4).ToList();
            return View("/Views/Shared/_RelatedProducts.cshtml", products);
        }
    }
}
