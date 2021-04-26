using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class User : MetaDataSupport
    {
        public User(string userName, long id, DateTime created, DateTime modified, Guid uuid)
        {
            UserName = userName;
            Id = id;
            Created = created;
            Modified = modified;
            Uuid = uuid;
            Passwords = new List<UserPasswordHistory>();
            QuoteRequests = new List<QuoteRequest>();
            UserTypes = new List<UserType>();
        }

        public static User Empty => new User("empty", -1, DateTime.Now, DateTime.Now, Guid.NewGuid());
        public long Id { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set;}
        public DateTime Modified { get; set;}
        public Guid Uuid { get; set;}
        public IList<UserPasswordHistory> Passwords { get; set;}
        public IList<QuoteRequest> QuoteRequests { get; set; }
        public IList<UserType> UserTypes { get; }
    }
}