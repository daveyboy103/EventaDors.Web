using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IUser
    {
        public long Id { get; }
        public string UserName { get; set; }
        public DateTime Created { get; }
        public DateTime Modified { get; }
        public Guid Uuid { get; }
        
        public IList<IUserPasswordHistory> Passwords { get; }
    }

    public interface IUserPasswordHistory
    {
        public string Password { get; }
        public Guid Uuid { get; }
        public DateTime Created { get; }
        public DateTime Expired { get; }
    }
}