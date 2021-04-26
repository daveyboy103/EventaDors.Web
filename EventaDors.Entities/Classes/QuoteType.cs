using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteType 
    {
        public string Name { get; }
        public string Notes { get; }
        public string Link { get; }
        public IList<QuoteSubType> SubTypes { get; }
    }
}