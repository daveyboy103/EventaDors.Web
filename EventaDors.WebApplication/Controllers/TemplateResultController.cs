using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class TemplateResultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}