using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class ElementDetailController : Controller
    {
        private readonly Wrapper _wrapper;

        public ElementDetailController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public IActionResult Index(string ElementId)
        {
            QuoteRequestElement quoteRequestElement = _wrapper.GetQuoteRequestElement(int.Parse(ElementId));
            return View(quoteRequestElement);
        }
    }
}