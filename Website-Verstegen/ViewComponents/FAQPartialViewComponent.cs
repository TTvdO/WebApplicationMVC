using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;

namespace Website_Verstegen.ViewComponents
{
    public class FAQPartialViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public FAQPartialViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //Gets The ContactPerson For Verstegen
            //var contactPerson = _context.People.Where(cp => cp.EmailAdres == "hhensen@verstegen.nl").SingleOrDefault();
            var contactPerson = _context.People.FirstOrDefault();
            return View("/Views/Shared/_ContactFAQPartial.cshtml", contactPerson);
        }
    }
}
