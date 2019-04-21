using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Data;
using Website_Verstegen.Models;

namespace Website_Verstegen.ViewComponents
{
    public class CertificateViewComponent : ViewComponent
    {
        private readonly DatabaseContext _context;

        public CertificateViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var certificates = _context.Certificates.Take(9).ToList();
            return View("/Views/Shared/_FooterPartial.cshtml", certificates);
        }
    }
}
