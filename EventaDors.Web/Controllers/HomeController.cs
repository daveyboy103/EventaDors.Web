using Microsoft.AspNetCore.Mvc;

namespace EventaDors.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}