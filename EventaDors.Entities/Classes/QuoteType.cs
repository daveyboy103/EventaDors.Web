using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public class QuoteType
    {
        public QuoteType()
        {
            SubTypes = new List<QuoteSubType>();
            Id = -1;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public IList<QuoteSubType> SubTypes { get; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}