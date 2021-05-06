using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EventaDors.Entities.Classes
{
    public class User : MetaDataSupport
    {
        private string _userName;

        public User()
        {
            Passwords = new List<UserPasswordHistory>();
            QuoteRequests = new List<QuoteRequest>();
            UserTypes = new List<UserType>();
            BlockedUsers = new List<User>();
            ChatHistory = new List<Chat>();
        }
        public User(string userName, long id, DateTime created, DateTime modified, Guid uuid): this()
        {
            UserName = userName;
            PrimaryEmail = userName;
            Id = id;
            Created = created;
            Modified = modified;
            Uuid = uuid;
        }

        public static User Empty => new("empty", -1, DateTime.Now, DateTime.Now, Guid.NewGuid());
        public long Id { get; set; }

        [DisplayName("User Name")]
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                    _userName = PrimaryEmail;
                return _userName;
            }
            set => _userName = value;
        }

        public string PrimaryEmail { get; set; }
        public string CurrentPassword { get; set; }
        public bool Verified { get; set; }
        public Guid UserKey { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Guid Uuid { get; set; }
        public IList<UserPasswordHistory> Passwords { get; set; }
        public IList<QuoteRequest> QuoteRequests { get; set; }
        public IList<UserType> UserTypes { get; }
        public IList<User> BlockedUsers { get; }
        public IList<Chat> ChatHistory { get; }
        public int EventCount { get; set; }

        public override string ToString()
        {
            return $"{UserName} - {PrimaryEmail}";
        }
    }
}