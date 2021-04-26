using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventaDors.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<AuthenticationController> _logger;
        private Wrapper _wrapper;

        public AuthenticationController(ILogger<AuthenticationController> logger, string connectionString)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, "AuthenticationController ctor called");
            _connectionString = connectionString;
        }

        [HttpGet]
        public LoginResult Login(string userName, string password)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.Authenticate(userName, password);
        }

        [HttpPost]
        public long CreateUser(User user)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.CreateUser(user.Id).Id;
        }
    }
}