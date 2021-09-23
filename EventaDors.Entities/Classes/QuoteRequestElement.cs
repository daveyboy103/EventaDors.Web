using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequestElement : QuoteElement
    {
        public int? QuoteRequestElementId { get; set; }
        public Guid? QuoteId { get; set; }
        public QuoteRequestEvent Parent { get; set; }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}