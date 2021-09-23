namespace EventaDors.Entities.Classes
{
    public class QuoteElementType
    {
        public QuoteElementType()
        {
            Id = -1;
        }
        public string Name { get; init; }
        public string Notes { get; init; }
        public string Link { get; init; }
        public int Id { get; set; }
    }
}