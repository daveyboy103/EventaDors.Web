using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Factories
{
    public class QuoteTypeFactory : IQuoteTypeFactory
    {
        public IList<IQuoteType> GetTree(int? quoteTypeId)
        {
            throw new System.NotImplementedException();
        }

        public int Create(IQuoteType quoteType)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(IQuoteType quoteType)
        {
            throw new System.NotImplementedException();
        }
    }
}