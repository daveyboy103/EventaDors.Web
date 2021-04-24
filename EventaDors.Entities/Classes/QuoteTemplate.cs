using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteTemplate : IQuoteTemplate
    {
        public QuoteTemplate()
        {
            Elements = new List<IQuoteElement>();
        }
        public string Name { get; set;  }
        public string Notes { get; set; }
        public IQuoteType Type { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public IList<IQuoteElement> Elements { get; }
    }
}