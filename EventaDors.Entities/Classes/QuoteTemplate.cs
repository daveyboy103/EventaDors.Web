using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public class QuoteTemplate : CreatedModifiedBase
    {
        public QuoteTemplate()
        {
            Events = new List<QuoteTemplateEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public QuoteType Type { get; set; }
        public QuoteSubType SubType { get; set; }
        public IList<QuoteTemplateEvent> Events { get; }
        public override string ToString()
        {
            return $"{Name} - {Events.Count} Events";
        }
    }

    public class QuoteTemplateEvent : CreatedModifiedBase
    {
        public QuoteTemplateEvent()
        {
            Elements = new List<QuoteElement>();
        }
        public int Id { get; set; }
        public int Order { get; set; }
        public Event Event { get; set; }

        public bool Exclude { get; set; }
        public IList<QuoteElement> Elements { get; }

        public override string ToString()
        {
            return $"{Event.Name} - {Order}";
        }
    }

    public class Event
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public abstract class CreatedModifiedBase
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}