using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventaDors.Web.Controllers
{
    public class StaticDataController : Controller
    {
        private readonly string _connectionString;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ILogger<StaticDataController> _logger;
        private Wrapper _wrapper;
        
        public StaticDataController(ILogger<StaticDataController> logger, string connectionString)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, "StaticDataController ctor called");
            _connectionString = connectionString;
        }

        [HttpPost("AddUpdateQuoteElement")]
        public QuoteElement AddUpdateQuoteElement(QuoteElement quoteElement)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.AddUpdateQuoteElement(quoteElement);
        }        
        
        [HttpPost("AddUpdateQuoteElementType")]
        public QuoteElementType AddUpdateQuoteElementType(QuoteElementType quoteElementType)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.AddUpdateQuoteElementType(quoteElementType);
        } 
        
        [HttpPost("AddUpdateQuoteType")]
        public QuoteType AddUpdateQuoteType(QuoteType quoteType)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.AddUpdateQuoteType(quoteType);
        }
        
        [HttpPost("AddUpdateQuoteSubType")]
        public QuoteSubType AddUpdateQuoteSubType(QuoteSubType quoteSubType)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.AddUpdateQuoteSubType(quoteSubType);
        }       
        
        [HttpPost("AddUpdateQuoteTemplate")]
        public QuoteTemplate AddUpdateQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.AddUpdateQuoteTemplate(quoteTemplate);
        }        
        
        [HttpPost("AddUpdateUserType")]
        public UserType AddUpdateUserType(UserType userType)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.AddUpdateUserType(userType);
        }
    }
}