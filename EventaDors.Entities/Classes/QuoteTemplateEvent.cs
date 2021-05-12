using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
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
}