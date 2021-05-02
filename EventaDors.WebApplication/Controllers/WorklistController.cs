using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class WorklistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}