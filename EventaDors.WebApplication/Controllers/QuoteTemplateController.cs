using System;
using System.Linq;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class QuoteTemplateController : Controller
    {
        private readonly Wrapper _wrapper;

        public QuoteTemplateController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index()
        {
            var id = Request.Query["id"];

            QuoteTemplateEvent ev = _wrapper.GetQuoteTemplateEvent(int.Parse(id));
            
            return View(ev);
        }

        [HttpGet]
        public IActionResult ChosenTemplate(int Id)
        {
            var quoteTemplate = _wrapper.GetQuoteTemplates(Id);

            foreach (var evt in quoteTemplate.First().Events)
            {
                var elements = _wrapper.GetQuoteTemplateEvent(evt.Id);
                foreach (var element in elements.Elements)
                {
                    evt.Elements.Add(element);
                }
            }
            
            return View(quoteTemplate.First());
        }

        [HttpPost]
        public IActionResult ProcessTemplate(QuoteTemplate quoteTemplate)
        {
            var form = Request.Form;
            string email = SessionHelper.GetString(Statics.EmailTempData);
            User user = _wrapper.CreateUser(email);
            Journey journey = _wrapper.GetJourney(email);
            int attendance = int.Parse(Request.Form["Attendees"]);
            var templateId = int.Parse(Request.Form["TemplateId"]);
            var quoteRequest = _wrapper.CreateRequestFromTemplate(templateId, user.Id, attendance, journey.EventDate);
            
            return View(quoteRequest);
        }
    }
}