using System.Collections.Generic;

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