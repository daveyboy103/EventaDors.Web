using EventaDors.WebApplication.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult LoggedIn()
        {
            return View();
        }
    }
}