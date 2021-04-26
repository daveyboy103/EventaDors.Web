using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteType 
    {
        public QuoteType()
        {
            SubTypes = new List<QuoteSubType>();
        }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public IList<QuoteSubType> SubTypes { get; }
    }
}