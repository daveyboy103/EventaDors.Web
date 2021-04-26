using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteTemplate 
    {
        public QuoteTemplate()
        {
            Elements = new List<QuoteElement>();
        }
        public string Name { get; set;  }
        public string Notes { get; set; }
        public QuoteType Type { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public IList<QuoteElement> Elements { get; }
    }
}