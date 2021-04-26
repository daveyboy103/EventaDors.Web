using System;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class UserPasswordHistory 
    {
        public string Password { get; }
        public Guid Uuid { get; }
        public DateTime Created { get; }
        public DateTime Expired { get; }
    }
}