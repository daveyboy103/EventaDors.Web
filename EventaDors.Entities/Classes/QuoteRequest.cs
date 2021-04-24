using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequest : IQuoteRequest
    {
        public QuoteRequest()
        {
            Elements = new List<IQuoteElement>();
        }
        public Guid QuoteId { get; }
        public string Name { get; }
        public string Notes { get; }
        public IQuoteType Type { get; }
        public DateTime Created { get; }
        public DateTime Submitted { get; }
        public IList<IQuoteElement> Elements { get; }
    }
}