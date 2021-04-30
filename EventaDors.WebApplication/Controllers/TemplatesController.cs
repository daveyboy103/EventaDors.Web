using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class TemplatesController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}