using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewModels
{
    public class ThemeProducts_ViewModel
    {
        public ThemeProduct CurrentThemeProduct { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Theme> Themes { get; set; }
    }
}
