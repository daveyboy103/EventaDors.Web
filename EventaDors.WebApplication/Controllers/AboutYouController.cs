using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class AboutYouController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}