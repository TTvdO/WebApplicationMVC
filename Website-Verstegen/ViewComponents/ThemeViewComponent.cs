using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;

namespace Website_Verstegen.ViewComponents
{
    public class ThemeViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public ThemeViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var themes = _context.Themes.Take(6).ToList();
            return View("/Views/Shared/_ThemesPartial.cshtml", themes);
        }
    }
}
