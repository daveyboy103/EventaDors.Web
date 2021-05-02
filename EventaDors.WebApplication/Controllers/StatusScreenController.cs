using System.Collections.Generic;
using System.Linq;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventaDors.WebApplication.Controllers
{
    public class StatusScreenController : Controller
    {
        private readonly Wrapper _wrapper;

        public StatusScreenController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }

        // GET
        public IActionResult Index(string UserName, string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = TempData["LoginUser"].ToString();
            }
            
            IList<Deadline> deadlines= null;
            var requests = _wrapper.GetRequestsForUser(int.Parse(UserId));

            if (requests.Count == 0)
            {
                return RedirectToAction("ContinueRegistration", "Register", UserId);
            }
            
            TempData.Remove("LoginUser");

            if (requests.Count() == 1)
            {
                deadlines = _wrapper.GetDeadlines(requests.First().QuoteIdIdentity, 26);
            }
            
            var ret = new MultiDataWrapper<IEnumerable<EventaDors.Entities.Classes.Deadline>>
            {
                List = new SelectList(requests, "QuoteIdIdentity", "Name"),
                Single = deadlines
            };
            
            return View(ret);
        }
    }
}