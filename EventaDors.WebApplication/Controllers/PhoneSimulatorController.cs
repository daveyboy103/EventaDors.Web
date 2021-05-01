using System.Linq;
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
            var users = new SelectList(_wrapper.ListUsers().Where(x => x.EventCount > 0), "Id", "UserName");
            return View(users);
        }
    }
}