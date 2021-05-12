using System.Linq;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class DirectoryController : Controller
    {
        // GET
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.All(x => x != Statics.EmailTempData))
            {
                return RedirectToAction("Index", "Registration");
            }
            
            return View();
        }
    }

    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult LoggedIn()
        {
            return View();
        }
    }

    public class GuestListController : Controller
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
    
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove(Statics.EmailTempData);
            return RedirectToAction("Index", "Registration");
        }
    }
}