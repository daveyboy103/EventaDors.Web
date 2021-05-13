using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequestEvent : QuoteTemplateEvent
    {
        public QuoteRequestEvent()
        {
            Elements = new List<QuoteRequestElement>();
            
            HelpMessages.Add(nameof(EventDate), 
                "This is the date of the event. This may differ from the main event date as some events happen in the run up period.");
            HelpMessages.Add(nameof(LeadWeeks),
               "This value is normally the maximum lead weeks value of the contained elements but can be overriden here for the event itself." );
            HelpMessages.Add(nameof(Name),
                "By default this is the name of the underlying template but it can be overriden to give the event more meaningful name.");
            HelpMessages.Add(nameof(Venue),
                "Events can be held at different venues, a private home for example. To set the venue for an event click Edit Event.");
        }
        public Guid QuoteId { get; set; }
        public DateTime EventDate { get; set; }
        public Venue Venue { get; set; }
        public int? Attendees { get; set; }
        public int? LeadWeeks { get; set; }
        public string Name  { get; set; }
        public IList<QuoteRequestElement> Elements { get; set; }
    }
}