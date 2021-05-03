using System.Runtime.InteropServices;
using EventaDors.Entities.Classes;
using EventaDors.Entities.Interfaces;

namespace EventaDors.DataManagement.Processors
{
    public class VenueProcessor : IElementProcessor
    {
        private readonly Wrapper _wrapper;

        public VenueProcessor(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        
        public bool Process(QuoteRequestElement request)
        {
            return true;
        }
    }
}