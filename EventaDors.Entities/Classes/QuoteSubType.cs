using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteSubType : CreatedModifiedBase
    {
        public QuoteSubType()
        {
            Id = -1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
    }
}