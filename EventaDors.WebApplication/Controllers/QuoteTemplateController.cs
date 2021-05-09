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
        public IActionResult ProcessTemplate(QuoteTemplate quoteTemplate)
        {
            var form = Request.Form;

            EventsElementExclusion exclusions = LoadExclusions(form);
            
            return View();
        }

        private EventsElementExclusion LoadExclusions(IFormCollection form)
        {
            var ret = new EventsElementExclusion();
            QuoteTemplateEvent currentEvent = null;
            
            foreach (string formKey in form.Keys)
            {
                var item = form[formKey];

                if (formKey.StartsWith("Event"))
                {
                    if (item[0] == "true")
                    {
                        currentEvent = new QuoteTemplateEvent{Id=int.Parse(formKey.Substring("Event_".Length))};
                        ret.Events.Add(currentEvent);
                    }

                    if (item[0] == "on")
                    {
                        currentEvent.Elements.Add(new QuoteElement{ Id = int.Parse(formKey.Substring(formKey.LastIndexOf("_")+ 1))});
                    }
                }
            }

            return ret;
        }
    }
}