using System.Linq;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class TaskListController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.All(x => x != Statics.EmailTempData))
            {
                return RedirectToAction("Index", "Registration");
            }
            
            return View();
        }
    }
}