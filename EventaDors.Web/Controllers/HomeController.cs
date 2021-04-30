using Microsoft.AspNetCore.Mvc;

namespace EventaDors.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _Layout = "sample";
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}