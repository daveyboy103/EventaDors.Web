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
            return null;
        }        
        
        [HttpPost("AddUpdateQuoteElementType")]
        public QuoteElementType AddUpdateQuoteElementType(QuoteElementType quoteElementType)
        {
            return null;
        } 
        
        [HttpPost("AddUpdateQuoteType")]
        public QuoteType AddUpdateQuoteType(QuoteType quoteType)
        {
            return null;
        }
        
        [HttpPost("AddUpdateQuoteElementSubType")]
        public QuoteSubType AddUpdateQuoteSubType(QuoteSubType quoteSubType)
        {
            return null;
        }       
        
        [HttpPost("AddUpdateQuoteTemplate")]
        public QuoteTemplate AddUpdateQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            return null;
        }        
        
        [HttpPost("AddUpdateUserType")]
        public UserType AddUpdateUserType(UserType userType)
        {
            return null;
        }
    }
}