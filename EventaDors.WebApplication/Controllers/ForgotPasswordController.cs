using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class ForgotPasswordController : Controller
    {
        public IActionResult Index(Journey journey)
        {
            return View(journey);
        }
    }
}