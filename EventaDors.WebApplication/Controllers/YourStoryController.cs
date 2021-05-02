using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class YourStoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}