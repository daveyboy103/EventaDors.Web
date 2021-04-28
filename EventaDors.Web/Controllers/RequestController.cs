using System;
using System.Collections.Generic;
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
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ILogger<RequestController> _logger;
        private readonly string _connectionString;
        private Wrapper _wrapper;

        public RequestController(ILogger<RequestController> logger, string connectionString)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, "RequestController ctor called");
            _connectionString = connectionString;
        }

        [HttpGet("GetDeadlines")]
        public IList<Deadline> GetDeadline(int quoteIdIdentity, int alarmThreshold)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.GetDeadlines(quoteIdIdentity, alarmThreshold);
        }

        [HttpPost("PickupQuoteRequestItem")]
        public QuoteRequestElementResponse PickupQuoteRequestItem(QuoteRequestElementResponse response)
        {
            QuoteRequestElementResponse ret;
            _wrapper = new Wrapper(_connectionString);
            ret = _wrapper.PickupQuoteRequestItem(response);
            return ret;
        }

        [HttpGet("CreateFromTemplate")]
        public QuoteRequest CreateFromTemplate(int templateId, int userId, int attendees, DateTime dueDate)
        {
            _wrapper = new Wrapper(_connectionString);
            QuoteRequest ret = _wrapper.CreateRequestFromTemplate(templateId, userId, attendees, dueDate);
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