using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class ContentController : Controller
    {
        public IActionResult Features()
        {
            return View();
        } 
        
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }
    }

    public class VenuesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 
    }
}