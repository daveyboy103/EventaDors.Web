using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly Wrapper _wrapper;

        public LoginController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        public IActionResult Index()
        {
            var loginUser = new User();
            return View(loginUser);
        }

        [HttpPost]
        public IActionResult ProcessLogin(User loginUser)
        {
            if (_wrapper.LoginUser(loginUser))
            {
                loginUser = _wrapper.CreateUser(loginUser.Id);
                TempData.Add("LoginUser", loginUser.Id.ToString());
                return RedirectToAction("Index", "StatusScreen", loginUser.Id);
            }

            return new ForbidResult();
        }
    }
}