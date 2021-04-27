using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequest : MetaDataSupport
    {
        public QuoteRequest()
        {
            Elements = new List<QuoteElement>();
        }

        public Guid QuoteId { get; set; }
        public int QuoteIdIdentity { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime? DueDate { get; set; }
        public QuoteType Type { get; set; }
        public QuoteSubType SubType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public IList<QuoteElement> Elements { get; }
    }
}