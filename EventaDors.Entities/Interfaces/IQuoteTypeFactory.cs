using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteTypeFactory
    {
        IList<IQuoteType> GetTree(int? quoteTypeId);
        int Create(IQuoteType quoteType);
        bool Delete(IQuoteType quoteType);
    }
}