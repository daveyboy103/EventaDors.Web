using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class User : IUser
    {
        public User(string userName, long id, DateTime created, DateTime modified, Guid uuid)
        {
            UserName = userName;
            Id = id;
            Created = created;
            Modified = modified;
            Uuid = uuid;
            Passwords = new List<IUserPasswordHistory>();
            QuoteRequests = new List<IQuoteRequest>();
        }

        public long Id { get; }
        public string UserName { get; set; }
        public DateTime Created { get; }
        public DateTime Modified { get; }
        public Guid Uuid { get; }
        public IList<IUserPasswordHistory> Passwords { get; }
        public IList<IQuoteRequest> QuoteRequests { get; }
    }
}