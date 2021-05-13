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
                foreach (var element in elements.TemplateElements)
                {
                    evt.TemplateElements.Add(element);
                }
            }
            
            return View(quoteTemplate.First());
        }

        [HttpPost]
        [HttpGet]
        public IActionResult ProcessTemplate(QuoteTemplate quoteTemplate)
        {
            if (HttpContext.Session.Keys.All(x => x != Statics.EmailTempData))
            {
                return RedirectToAction("Index", "Registration");
            }
            
            QuoteRequest quoteRequest = null;
            
            if(!Request.Query.ContainsKey("quoteIdIdentity"))
            {
                string email = (HttpContext.Session.Keys.Contains(Statics.EmailTempData)
                    ? HttpContext.Session.GetString(Statics.EmailTempData)
                    : String.Empty);
                User user = _wrapper.CreateUser(email);
                Journey journey = _wrapper.GetJourney(email);

                var templateId = 0;
                if (Request.Method == "POST")
                {
                    int attendance = int.Parse(Request.Form["Attendees"]);
                    templateId = int.Parse(Request.Form["TemplateId"]);
                    quoteRequest =
                        _wrapper.CreateRequestFromTemplate(templateId, user.Id, attendance, journey.EventDate);
                    return View(quoteRequest);
                }

                if (!journey.QuoteIdIdentity.HasValue)
                {
                    if (journey.Completed.HasValue)
                    {
                        if(!HttpContext.Session.Keys.Contains(Statics.EmailTempData))
                            HttpContext.Session.SetString(Statics.EmailTempData, journey.Email);
                        return RedirectToAction("Step3", "Registration");
                    }
                }
                else
                {
                    templateId = journey.QuoteIdIdentity.Value;
                
                    if (Request.Method == "GET")
                    {
                        quoteRequest = _wrapper.LoadQuoteRequest(templateId);
                    }
                }
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