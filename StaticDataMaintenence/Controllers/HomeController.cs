using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StaticDataMaintenence.Models;
using Action = System.Action;

namespace StaticDataMaintenence.Controllers
{
    public class HomeController : Controller
    {
        private readonly Wrapper _wrapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(Wrapper wrapper, ILogger<HomeController> logger)
        {
            _logger = logger;
            _wrapper = wrapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Events(EventWrapper evtWrapper)
        {
            EventWrapper ret = new EventWrapper();
            
            FormAction action = GetAction(Request, "eventId");
            IList<QuoteSubType> subTypes = _wrapper.ListSubTypes();
            ret.SubTypes = subTypes;
            
            switch (action)
            {
                case FormAction.Delete:
                    break; 
                case FormAction.Load:
                    break;
                case FormAction.Get:
                    break; 
                case FormAction.New:
                    ret.Action = FormAction.New;
                    ret.Event = new Event()
                    {
                        Id= -1
                    };
                    break;                
                case FormAction.Save:
                    ret.Action = FormAction.New;
                    ret.Event = _wrapper.SaveEvent(evtWrapper.Event);
                    ret.Action = FormAction.Success;
                    break;
            }
            
            ret.List = _wrapper.ListEvents();
            
            return View(ret);
        }

        private FormAction GetAction(HttpRequest request, string lookupKey)
        {
            FormAction ret = FormAction.Save;
            
            if (Request.Query.ContainsKey("new"))
            {
                ret = FormAction.New;
            } 
            if (Request.Query.ContainsKey("load"))
            {
                ret = FormAction.Load;
            }

            if (Request.Query.ContainsKey(lookupKey))
            {
                ret = FormAction.Get;
            } 
            
            if (Request.Query.ContainsKey("delete"))
            {
                ret = FormAction.Delete;
            }

            return ret;
        }

        public IActionResult GuestGroups()
        {
            return View();
        }

        public IActionResult QuoteElements()
        {
            return View();
        }

        public IActionResult SpecialNeeds()
        {
            return View();
        }

        public IActionResult GiftRegistry()
        {
            return View();
        }

        public IActionResult QuoteSubStype()
        {
            return View();
        }

        public IActionResult QuoteTemplates()
        {
            return View();
        }

        public IActionResult QuoteTemplateEvents()
        {
            return View();
        }

        public IActionResult QuoteTypes()
        {
            return View();
        }

        public IActionResult SaveEvent()
        {
            return RedirectToAction("Events");
        }

        public IActionResult NewEvent()
        {
            return RedirectToAction("Events");
        }

        public IActionResult LoadEvent()
        {
            var eventList = _wrapper.ListEvents();
            Event evt = _wrapper.ListEvents(int.Parse(Request.Query["eventid"])).First();
            IList<QuoteSubType> subTypes = _wrapper.ListSubTypes();
            EventWrapper evtWrapper = new EventWrapper(evt, eventList);
            evtWrapper.SubTypes = subTypes;
            // return RedirectToAction("Events", "Home", evtWrapper);
            //return Events(evtWrapper);
            return View("Events", evtWrapper);
        }
    }
}