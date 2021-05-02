using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class Journey1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}