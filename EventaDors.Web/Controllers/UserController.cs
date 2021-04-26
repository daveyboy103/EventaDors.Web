using System;
using System.Data.SqlClient;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventaDors.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<UserController> _logger;
        private Wrapper _wrapper;

        public UserController(ILogger<UserController> logger, string connectionString)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, "UserController ctor called");
            _connectionString = connectionString;
        }

        [HttpGet("GetUser")]
        public User GetUser(int id)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.CreateUser(id);
        }

        [HttpGet("DeleteUser")]
        public bool DeleteUser(long userID, bool deactivateOnly)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.DeleteUser(userID, deactivateOnly);
        }

        [HttpGet("ChangePassword")]
        public bool ChangePassword(long userId, string oldPassword, string newPassword)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.ChangePassword(userId, oldPassword, newPassword);
        }

        [HttpPost("UpdateUser")]
        public User UpdateUser(User user)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.UpdateUser(user);
        }

        [HttpPost("RegisterUser")]
        public User RegisterUser(User user)
        {
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.RegisterUser(user);
        }

        [HttpPost("VerifyUser")]
        public bool VerifyUser(string uuid)
        {
            Guid guid = new Guid(uuid);
            _wrapper = new Wrapper(_connectionString);
            return _wrapper.VerifyUser(guid);
        }
    }
}