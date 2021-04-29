using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteSubType
    {
        public QuoteSubType()
        {
            Id = -1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}