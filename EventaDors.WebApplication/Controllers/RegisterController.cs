using System;
using System.Diagnostics.Eventing.Reader;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Http;
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
            if (!journey.EventDate.HasValue)
            {
                _wrapper.RegisterUser(journey.User);
            }

            return View("ContinueRegistration", journey);
        } 
        public IActionResult ContinueRegistration(string userId, Journey journey)
        {
            if (string.IsNullOrEmpty(userId))
                userId = HttpContext.Session.GetString(Statics.LogonUserKey);
            var user = _wrapper.CreateUser(long.Parse(userId));
            HttpContext.Session.SetString(Statics.UserIdKey, userId);
            return ProcessRegistration(new Journey
            {
                User = user,
                ContactNumber = SessionHelper.GetString(Statics.ContactNumber),
                FirstName = SessionHelper.GetString(Statics.FirstName),
                Surname = SessionHelper.GetString(Statics.Surname),
                PartnerEmail = SessionHelper.GetString(Statics.PartnerEmail),
                PostalCode = SessionHelper.GetString(Statics.PostalCode),
                EventDate = GetEventDate()
            });
        }

        private static DateTime GetEventDate()
        {
            if(!string.IsNullOrEmpty(SessionHelper.GetString(Statics.EventDate)))
                return DateTime.Parse(SessionHelper.GetString(Statics.EventDate));
            return DateTime.Today.AddDays(365);
        }
    }
}