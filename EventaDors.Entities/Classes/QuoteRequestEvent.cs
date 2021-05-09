using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequestEvent : QuoteTemplateEvent
    {
        public Guid QuoteId { get; set; }
        public DateTime EventDate { get; set; }
        public Venue Venue { get; set; }
        public int? Attendees { get; set; }
        public int? LeadWeeks { get; set; }
    }
}