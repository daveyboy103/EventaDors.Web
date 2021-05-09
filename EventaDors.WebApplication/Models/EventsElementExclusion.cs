using System.Collections.Generic;
using EventaDors.Entities.Classes;

namespace EventaDors.WebApplication.Models
{
    public class EventsElementExclusion
    {
        public EventsElementExclusion()
        {
            Events = new List<QuoteTemplateEvent>();
        }
        public IList<QuoteTemplateEvent> Events { get; set; }
    }
}