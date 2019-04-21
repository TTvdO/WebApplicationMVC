using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website_Verstegen.Controllers
{
    public class UnderConstructionController : Controller
    {
        public UnderConstructionController()
        {

        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
