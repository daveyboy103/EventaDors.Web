using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequestElement : QuoteElement
    {
        public int? QuoteRequestElementId { get; init; }
        public Guid? QuoteId { get; init; }
        public QuoteRequestEvent Parent { get; init; }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}