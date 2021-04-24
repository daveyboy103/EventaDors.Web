using System;

namespace EventaDors.Entities.Interfaces
{
    public interface IUserPasswordHistory
    {
        public string Password { get; }
        public Guid Uuid { get; }
        public DateTime Created { get; }
        public DateTime Expired { get; }
    }
}