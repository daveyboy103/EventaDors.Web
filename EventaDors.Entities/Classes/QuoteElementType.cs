namespace EventaDors.Entities.Classes
{
    public class QuoteElementType
    {
        public QuoteElementType()
        {
            Id = -1;
        }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public int Id { get; set; }
    }
}