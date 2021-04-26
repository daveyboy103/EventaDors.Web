using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequest : MetaDataSupport
    {
        public QuoteRequest()
        {
            Elements = new List<QuoteElement>();
        }
        
        public Guid QuoteId { get; }
        public string Name { get; }
        public string Notes { get; }
        public QuoteType Type { get; }
        public DateTime Created { get; }
        public DateTime Submitted { get; }
        public IList<QuoteElement> Elements { get; }
    }
}