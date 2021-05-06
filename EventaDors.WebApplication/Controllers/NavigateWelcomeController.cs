using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class NavigateWelcomeController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            if (journey == null)
                journey = new Journey();

            journey.CurrentPage = "Welcome";

            return View(journey);
        }
    }
    
    public class NavigateLoginController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            journey.CurrentPage = "Login";
            return View(journey);
        }
    }    
    
    public class NavigateRegisterController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            journey.CurrentPage = "Register";
            return View(journey);
        }
    }
    
    public class NavigateAboutYouController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            journey.CurrentPage = "About You";
            return View(journey);
        }
    } 
    
    public class NavigateChooseTemplateController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            journey.CurrentPage = "Choose Template";
            return View(journey);
        }
    }
    
    public class NavigateFinishController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            journey.CurrentPage = "Choose Template";
            return View(journey);
        }
    }
}