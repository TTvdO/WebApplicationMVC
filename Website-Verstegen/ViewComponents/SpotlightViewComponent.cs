using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Website_Verstegen.ViewComponents
{
    public class SpotlightViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public SpotlightViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //Vind juiste spotlightconnector adv actionname & controllername
            string actionName = this.ViewContext.RouteData.Values["action"].ToString();
            string controllerName = this.ViewContext.RouteData.Values["controller"].ToString();
            //Haal de laatst aangemaakte spotlight op die voor de 
            var spotlightConnection = _context.SpotLightConnectors.Where(sc => sc.actionName == actionName).Where(sc => sc.controllerName == controllerName).Last();

            //Vind alle spotlightcontent die het Id hebben van de spotlightconnector
            List<SpotlightContent> cts = _context.SpotlightContents.Where(sc => sc.SpotlightConnectorId == spotlightConnection.Id).ToList();

            //lijst aanmaken met items die op scherm getoond worden
            var items = new List<SpotLightItem>();
            foreach(SpotlightContent item in cts)
            {
                //controleren op type en entiteit ophalen waarvan de id overeenkomt met die van de spotlightcontent
                switch (item.SpType)
                {
                    case "Product":
                        var Product = _context.Products.FirstOrDefault(m => m.Id == item.SpId);
                        if (Product != null) { items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(Product))); };
                        break;
                    case "Packaging":
                        var Packaging = _context.Packagings.FirstOrDefault(m => m.Id == item.SpId);
                        if (Packaging != null) { items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(Packaging))); };
                        break;
                    case "Recipe":
                        var Recipe = _context.Recipes.FirstOrDefault(m => m.Id == item.SpId);
                        if (Recipe != null) { items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(Recipe))); };
                        break;
                    case "Blog":
                        var Blogs = _context.Blogs.FirstOrDefault(m => m.Id == item.SpId);
                        if (Blogs != null) { items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(Blogs))); };
                        break;
                    case "Story":
                        var Stories = _context.Stories.FirstOrDefault(m => m.Id == item.SpId);
                        if(Stories != null)
                        {
                            items.Add(JsonConvert.DeserializeObject<SpotLightItem>(JsonConvert.SerializeObject(Stories)));
                        };
                        break;
                }
            }

            return View("/Views/Shared/_SpotlightPartial.cshtml", items);

        }
    }
}