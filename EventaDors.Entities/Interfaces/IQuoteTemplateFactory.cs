namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteTemplateFactory
    {
        IQuoteTemplate GetTemplate(string name, int? id);
        IQuoteTemplate Create(IQuoteTemplate quoteTemplate);
        bool Delete(IQuoteTemplate quoteTemplate);
    }
}