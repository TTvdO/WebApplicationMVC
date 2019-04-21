using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewModels
{
    public class SpotlightViewModel
    {
        public SpotLightConnector SpotLightConnector { get; set; }
        public List<SpotLightItem> SpotLightItems { get; set; }
    }
}
