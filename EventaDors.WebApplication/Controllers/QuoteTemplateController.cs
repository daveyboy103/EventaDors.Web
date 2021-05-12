using System;
using System.Linq;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
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
        [HttpGet]
        public IActionResult ProcessTemplate(QuoteTemplate quoteTemplate)
        {
            QuoteRequest quoteRequest = null; 
            if(!Request.Query.ContainsKey("quoteIdIdentity"))
            {
                var form = Request.Form;
                string email = (HttpContext.Session.Keys.Contains(Statics.EmailTempData)
                    ? HttpContext.Session.GetString(Statics.EmailTempData)
                    : String.Empty);
                User user = _wrapper.CreateUser(email);
                Journey journey = _wrapper.GetJourney(email);
                int attendance = int.Parse(Request.Form["Attendees"]);
                var templateId = int.Parse(Request.Form["TemplateId"]);
                quoteRequest =
                    _wrapper.CreateRequestFromTemplate(templateId, user.Id, attendance, journey.EventDate);
            }
            else
            {
                int id = int.Parse(Request.Query["quoteIdIdentity"]);
                quoteRequest = _wrapper.LoadQuoteRequest(id);
            }

            return View(quoteRequest);
        }

        public IActionResult ProcessEventUpdates()
        {
            return View();
        }

        public IActionResult EditEventDetails()
        {
            var quoteEvent = new QuoteRequestEvent();
            int eventId = int.Parse(Request.Query["eventId"]);
            int quoteId = int.Parse(Request.Query["quoteId"]);
            return View(quoteEvent);
        }
    }
}