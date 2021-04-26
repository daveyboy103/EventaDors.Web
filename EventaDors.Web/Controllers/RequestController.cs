using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventaDors.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly string _connectionString;
        private Wrapper _wrapper;

        public RequestController(ILogger<UserController> logger, string connectionString)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, "RequestController ctor called");
            _connectionString = connectionString;
        }

        [HttpGet("CreateFromTemplate")]
        public QuoteRequest CreateFromTemplate(int templateId, int userId, int attendees)
        {
            _wrapper = new Wrapper(_connectionString);
            QuoteRequest ret = _wrapper.CreateRequestFromTemplate(templateId, userId, attendees);
            return ret;
        }

        [HttpGet("LoadRequest")]
        public QuoteRequest LoadRequest(int quoteIdIdentity)
        {
            _wrapper = new Wrapper(_connectionString);
            QuoteRequest ret = _wrapper.LoadQuoteRequest(quoteIdIdentity);
            return ret;
        }
    }
}