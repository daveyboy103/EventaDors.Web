using Microsoft.AspNetCore.Mvc;

namespace EventaDors.Web.Controllers
{
    public class PrivacyController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}