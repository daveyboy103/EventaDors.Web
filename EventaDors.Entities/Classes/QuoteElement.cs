using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class QuoteElement : IQuoteElement
    {
        public IQuoteElementType Type { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public double Budget { get; set; }
        public double BudgetTolerance { get; set; }
        public double Quantity { get; set; }
        public int Id { get; set; }
    }
}