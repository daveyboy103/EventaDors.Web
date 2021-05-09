using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequest : MetaDataSupport
    {
        public QuoteRequest()
        {
            Events = new List<QuoteRequestEvent>();
        }

        public Guid QuoteId { get; set; }
        public int QuoteIdIdentity { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime? DueDate { get; set; }
        public QuoteType Type { get; set; }
        public QuoteSubType SubType { get; set; }
        public IList<QuoteRequestEvent> Events { get; }
        public User Owner { get; set; }
        public int Attendees { get; set; }

        public override string ToString()
        {
            return $"{Name} - Events {Events.Count} - Attendees {Attendees}";
        }
    }
}