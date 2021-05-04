using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class MessageController : Controller
    {
        // GET
        public IActionResult Index()
        {
            var id = HttpContext.Session.Id;
            SessionIdWrapper idWrapper = new SessionIdWrapper(id);
            return View(idWrapper);
        }
    }
}