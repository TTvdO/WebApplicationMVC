using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.ViewModels;

namespace Website_Verstegen.ViewComponents
{
    public class OverviewMenuViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public OverviewMenuViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories.ToList();

            return View("/Views/Shared/_OverviewMenuPartial.cshtml", categories);
        }
    }
}
