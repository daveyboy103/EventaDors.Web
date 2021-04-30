using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class StatusScreenController : Controller
    {
        // GET
        public IActionResult Index(User user)
        {
            return View();
        }
    }
}