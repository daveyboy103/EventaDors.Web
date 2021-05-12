using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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
                ContactNumber = (HttpContext.Session.Keys.Contains(Statics.ContactNumber) ? HttpContext.Session.GetString(Statics.ContactNumber) : String.Empty),
                FirstName = (HttpContext.Session.Keys.Contains(Statics.FirstName) ? HttpContext.Session.GetString(Statics.FirstName) : String.Empty),
                Surname = (HttpContext.Session.Keys.Contains(Statics.Surname) ? HttpContext.Session.GetString(Statics.Surname) : String.Empty),
                PartnerEmail = (HttpContext.Session.Keys.Contains(Statics.PartnerEmail) ? HttpContext.Session.GetString(Statics.PartnerEmail) : String.Empty),
                PostalCode = (HttpContext.Session.Keys.Contains(Statics.PostalCode) ? HttpContext.Session.GetString(Statics.PostalCode) : String.Empty),
                EventDate = GetEventDate()
            });
        }

        private DateTime GetEventDate()
        {
            var dateString = (HttpContext.Session.Keys.Contains(Statics.EventDate) ? HttpContext.Session.GetString(Statics.EventDate) : String.Empty);
            if(!string.IsNullOrEmpty(dateString))
                return DateTime.Parse(dateString);
            return DateTime.Today.AddDays(365);
        }
    }
}