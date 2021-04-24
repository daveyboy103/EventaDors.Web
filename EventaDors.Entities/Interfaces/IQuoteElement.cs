namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteElement
    {
        IQuoteElementType Type { get; }
        string Name { get; }
        string Notes { get; }
        double Budget { get; }
        double BudgetTolerance { get; }
        double Quantity { get; }
        int Id { get; }
    }
}