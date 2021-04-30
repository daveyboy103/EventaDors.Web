using EventaDors.Entities.Classes;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Processors
{
    public class VenueProcessor : IElementProcessor
    {
        public bool Process(QuoteRequestElement request)
        {
            return true;
        }
    }
    
    
}