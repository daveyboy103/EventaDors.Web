using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class RegisterController : Controller
    {
        private readonly Wrapper _wrapper;

        public RegisterController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessRegistration(Journey journey)
        {
            var ret = _wrapper.RegisterUser(journey.User);

            return View(journey);
        } 
        public IActionResult ContinueRegistration(string userId)
        {
            try
            {
                userId = TempData["LoginUser"].ToString();
                var user = _wrapper.CreateUser(long.Parse(userId));
                return View("ProcessRegistration", new Journey{ User = user});
            }
            finally
            {
                TempData.Remove("LoginUser");
            }
        }
    }
}