using System.Collections.Generic;

namespace Website_Verstegen.Models
{
    public class SpotLightConnector
    {
        public int Id { get; set; }
        public string actionName { get; set; }
        public string controllerName { get; set; }

        public List<SpotlightContent> SpotlightContent { get; set; }
    }
}