using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteSubType : IQuoteSubType
    {
        public string Name { get; }
        public string Notes { get; }
        public string Link { get; }
    }
}