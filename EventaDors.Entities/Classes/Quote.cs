using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class Quote : IQuote
    {
        public Quote()
        {
            Elements = new List<IQuoteElement>();
        }
        public IUser Owner { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Submitted { get; set; }
        public string Notes { get; set; }
        public IQuoteType Type { get; set; }
        public IList<IQuoteElement> Elements { get; }
    }
}