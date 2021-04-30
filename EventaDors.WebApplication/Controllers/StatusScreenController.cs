using EventaDors.DataManagement;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class StatusScreenController : Controller
    {
        private readonly Wrapper _wrapper;

        public StatusScreenController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index(string UserName, string UserId)
        {
            var requests = _wrapper.GetRequestsForUser(int.Parse(UserId));
            return View();
        }
    }
}