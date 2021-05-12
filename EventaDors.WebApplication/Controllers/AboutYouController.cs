using System;
using System.Linq;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace EventaDors.WebApplication.Controllers
{
    public class AboutYouController : Controller
    {
        public IActionResult Index(Journey journey)
        {
            if (journey.IsEmpty)
            {

                journey.Title = (HttpContext.Session.Keys.Contains(Statics.Title) ? HttpContext.Session.GetString(Statics.Title) : string.Empty) ;
                journey.FirstName = (HttpContext.Session.Keys.Contains(Statics.Title) ? HttpContext.Session.GetString(Statics.FirstName) : String.Empty);
                journey.FirstName = (HttpContext.Session.Keys.Contains(Statics.Surname) ? HttpContext.Session.GetString(Statics.Surname) : String.Empty);
                journey.FirstName = (HttpContext.Session.Keys.Contains(Statics.PostalCode) ? HttpContext.Session.GetString(Statics.PostalCode) : String.Empty);
                journey.FirstName = (HttpContext.Session.Keys.Contains(Statics.ContactNumber) ? HttpContext.Session.GetString(Statics.ContactNumber) : String.Empty);
                journey.FirstName = (HttpContext.Session.Keys.Contains(Statics.PartnerEmail) ? HttpContext.Session.GetString(Statics.PartnerEmail) : String.Empty);
                journey.FirstName = (HttpContext.Session.Keys.Contains(Statics.EventDate) ? HttpContext.Session.GetString(Statics.EventDate) : String.Empty);
            }
            
            HttpContext.Session.SetString(Statics.Title, journey.Title);
            HttpContext.Session.SetString(Statics.FirstName, journey.FirstName);
            HttpContext.Session.SetString(Statics.Surname, journey.Surname);
            HttpContext.Session.SetString(Statics.PostalCode, journey.PostalCode);
            HttpContext.Session.SetString(Statics.ContactNumber, journey.ContactNumber);
            HttpContext.Session.SetString(Statics.PartnerEmail, journey.PartnerEmail);
            HttpContext.Session.SetString(Statics.EventDate, journey.EventDate.Value.ToShortDateString());

            if (string.IsNullOrEmpty(journey.CurrentPage))
            {
                journey.CurrentPage = "Story";
                return View(journey);
            }

            if (journey.CurrentPage == "Story")
            {
                journey.CurrentPage = null;
                return RedirectToAction("ContinueRegistration", "Register", journey);
            }

            return null;
        }
    }
}