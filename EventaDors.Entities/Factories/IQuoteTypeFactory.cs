using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Factories
{
    public interface IQuoteTypeFactory
    {
        IList<IQuoteType> GetTree(int? quoteTypeId);
        int Create(IQuoteType quoteType);
        bool Delete(IQuoteType quoteType);
    }
}