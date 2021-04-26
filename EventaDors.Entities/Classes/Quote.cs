using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class Quote : MetaDataSupport
    {
        public Quote()
        {
            Elements = new List<QuoteElement>();
        }
        
        public User Owner { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Submitted { get; set; }
        public string Notes { get; set; }
        public QuoteType Type { get; set; }
        public IList<QuoteElement> Elements { get; }
    }
}