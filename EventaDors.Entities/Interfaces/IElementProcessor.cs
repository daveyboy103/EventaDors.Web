using EventaDors.Entities.Classes;

namespace EventaDors.Entities.Interfaces
{
    public interface IElementProcessor
    {
        bool Process(QuoteRequestElement request);
    }
}