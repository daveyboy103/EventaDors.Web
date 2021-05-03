using System;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class AboutYouController : Controller
    {
        public IActionResult Index(Journey journey)
        {
            if (journey.IsEmpty)
            {
                journey.Title = SessionHelper.GetString(Statics.Title);
                journey.FirstName = SessionHelper.GetString(Statics.FirstName);
                journey.Surname = SessionHelper.GetString(Statics.Surname);
                journey.PostalCode = SessionHelper.GetString(Statics.PostalCode);
                journey.ContactNumber = SessionHelper.GetString(Statics.ContactNumber);
                journey.PartnerEmail = SessionHelper.GetString(Statics.PartnerEmail);
                journey.EventDate = DateTime.Parse(SessionHelper.GetString(Statics.EventDate));
            }
            
            SessionHelper.SetString(Statics.Title, journey.Title);
            SessionHelper.SetString(Statics.FirstName, journey.FirstName);
            SessionHelper.SetString(Statics.Surname, journey.Surname);
            SessionHelper.SetString(Statics.PostalCode, journey.PostalCode);
            SessionHelper.SetString(Statics.ContactNumber, journey.ContactNumber);
            SessionHelper.SetString(Statics.PartnerEmail, journey.PartnerEmail);
            SessionHelper.SetString(Statics.EventDate, journey.EventDate.Value.ToShortDateString());

            if (string.IsNullOrEmpty(journey.NextPage))
            {
                journey.NextPage = "Story";
                return View(journey);
            }

            if (journey.NextPage == "Story")
            {
                journey.NextPage = null;
                return RedirectToAction("ContinueRegistration", "Register", journey);
            }

            return null;
        }
    }
}