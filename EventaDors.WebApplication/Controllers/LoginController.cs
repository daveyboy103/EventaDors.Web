using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public IActionResult ProcessLogin(User loginUser)
        {
            if (_wrapper.LoginUser(loginUser))
            {
                loginUser = _wrapper.CreateUser(loginUser.Id);
                HttpContext.Session.SetString(Statics.LogonUserKey, loginUser.Id.ToString());
                return RedirectToAction("Index", "StatusScreen", loginUser.Id);
            }

            return new ForbidResult();
        }
    }
}