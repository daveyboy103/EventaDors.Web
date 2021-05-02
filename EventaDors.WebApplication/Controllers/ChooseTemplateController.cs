using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class ChooseTemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}