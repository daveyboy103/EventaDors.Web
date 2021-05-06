using EventaDors.WebApplication.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            SessionHelper.Context = HttpContext;
            TempDataHelper.TempDataDictionary = TempData;
            TempData.Clear();
            
            return View();
        }
    }
}