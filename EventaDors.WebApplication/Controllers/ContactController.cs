using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}