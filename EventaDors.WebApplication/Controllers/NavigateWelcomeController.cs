using System;
using System.Linq;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class NavigateWelcomeController : Controller
    {
        private readonly Wrapper _wrapper;

        public NavigateWelcomeController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index(Journey journey)
        {
            var email = Request.Query["email"];

            if (email.Count != 0)
            {
                journey = _wrapper.GetJourney(email[0]);
                TempDataHelper.Set(Statics.EmailTempData, email);
            }else
            {
                string emailObj = TempDataHelper.Get(Statics.EmailTempData);
                
                if (emailObj != null)
                {
                    journey = _wrapper.GetJourney(emailObj);
                }
                else
                {
                    journey = new Journey();
                }
            }

            journey.CurrentPage = "Welcome";
            
            return View(journey);
        }
    }
    
    public class NavigateLoginController : Controller
    {
        private readonly Wrapper _wrapper;

        public NavigateLoginController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index(Journey journey)
        {
            var emailObj = TempDataHelper.Get(Statics.EmailTempData);

            if (!string.IsNullOrEmpty(emailObj))
            {
                journey = _wrapper.GetJourney(emailObj);
                journey.CurrentPage = "Login";

                if (!journey.Registered)
                    return RedirectToAction("Index", "NavigateRegister");
                _wrapper.PutJourney(journey);
                return View(journey);
            }
            return View(journey);
        }
    }    
    
    public class NavigateRegisterController : Controller
    {
        private readonly Wrapper _wrapper;

        public NavigateRegisterController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index(Journey journey)
        {
            var emailObj = TempDataHelper.Get(Statics.EmailTempData);
            
            if(!string.IsNullOrEmpty(emailObj))
                journey = _wrapper.GetJourney(emailObj);
            else
            {
                journey = new Journey();
            }
            
            journey.CurrentPage = "Register";

            return View(journey);
        }
    }
    
    public class NavigateAboutYouController : Controller
    {
        private readonly Wrapper _wrapper;

        public NavigateAboutYouController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        [HttpPost("Index")]
        public IActionResult Index(Journey journey)
        {
            var emailObj = TempDataHelper.Get(Statics.EmailTempData);
            TempDataHelper.Remove(Statics.EmailTempData);

            string password = journey.Password;
            journey = _wrapper.GetJourney(emailObj);
            journey.Password = password;
            
            journey.CurrentPage = "About You";
            
            if (!journey.Registered)
            {
                var user = new User(journey.Email, 0, DateTime.Now, DateTime.Now, Guid.NewGuid())
                {
                    PrimaryEmail = journey.Email,
                    CurrentPassword = journey.Password
                };
                
                _wrapper.RegisterUser(user);
                journey.Registered = true;
                _wrapper.PutJourney(journey);
            }

            return View(journey);
        }
    } 
    
    public class NavigateChooseTemplateController : Controller
    {
        // GET
        private readonly Wrapper _wrapper;

        public NavigateChooseTemplateController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public IActionResult Index(Journey journey)
        {
            if (!_wrapper.LoginUser(new User()
            {
                PrimaryEmail = journey.Email,
                CurrentPassword = journey.Password

            }))
            {
                return RedirectToAction("Index", "NavigateLogin");
            }
                
            journey = _wrapper.GetJourney(journey.Email);
            TempDataHelper.Set(Statics.EmailTempData, journey.Email);

            if (string.IsNullOrEmpty(journey.FirstName))
            {
                return RedirectToAction("Index", "NavigateAboutYou");
            }
            
            return View(journey);
        }
        
    }
    
    public class NavigateFinishController : Controller
    {
        private readonly Wrapper _wrapper;

        public NavigateFinishController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index(Journey journey)
        {
            _wrapper.PutJourney(journey);
            
            return View(journey);
        }
    }
}