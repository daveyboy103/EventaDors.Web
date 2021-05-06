using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventaDors.WebApplication.Helpers
{
    public static class SessionHelper
    {
        public static HttpContext Context { get; set; }
        
        public static void SetString(string key, string value)
        {
            if(!string.IsNullOrEmpty(value))
                Context.Session.SetString(key, value);
        }

        public static string GetString(string key)
        {
            if (Context.Session.Keys.Contains(key))
            {
                return Context.Session.GetString(key);
            }

            return null;
        }
    }
}