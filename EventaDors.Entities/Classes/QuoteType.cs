using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteType : IQuoteType
    {
        public string Name { get; }
        public string Notes { get; }
        public string Link { get; }
        public IList<IQuoteSubType> SubTypes { get; }
    }
}