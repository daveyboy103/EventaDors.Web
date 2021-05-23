using System;

namespace EventaDors.Entities.Classes
{
    public class Venue : CreatedModifiedBase
    {
        public int Id { get; set; }
        public Guid QuoteId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostCode { get; set; }
        public string PostTown { get; set; }
        public string Country { get; set; }
        public string ContactNumber { get; set; }
        public string MapLink { get; set; }
        public string SiteLink { get; set; }
        public override string ToString()
        {
            return $"{Name} - {PostTown}, {PostCode}, {Country}: Tel: {ContactNumber}";
        }
    }
}