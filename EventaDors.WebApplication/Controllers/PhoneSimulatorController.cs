using EventaDors.DataManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventaDors.WebApplication.Controllers
{
    public class PhoneSimulatorController : Controller
    {
        private readonly Wrapper _wrapper;

        public PhoneSimulatorController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index()
        {
            var users = new SelectList(_wrapper.ListUsers(), "Id", "UserName");
            return View(users);
        }
    }
}