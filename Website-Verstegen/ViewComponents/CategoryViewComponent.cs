using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website_Verstegen.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website_Verstegen.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public CategoryViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories.Take(14).ToList();
            return View("/Views/Shared/_CategoriesListPartial.cshtml", categories);
        }
    }
}
