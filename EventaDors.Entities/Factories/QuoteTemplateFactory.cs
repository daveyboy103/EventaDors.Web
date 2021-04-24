using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Factories
{
    public class QuoteTemplateFactory : IQuoteTemplateFactory
    {
        public IQuoteTemplate GetTemplate(string name, int? id)
        {
            throw new System.NotImplementedException();
        }

        public IQuoteTemplate Create(IQuoteTemplate quoteTemplate)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(IQuoteTemplate quoteTemplate)
        {
            throw new System.NotImplementedException();
        }
    }
}