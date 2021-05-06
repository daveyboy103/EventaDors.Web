using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class YourStoryController : Controller
    {
        public IActionResult Index(Journey journey)
        {
            return View(journey);
        }
    }
}