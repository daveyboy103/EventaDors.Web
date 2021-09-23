using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequest : MetaDataSupport
    {
        public QuoteRequest()
        {
            Events = new List<QuoteRequestEvent>();
        }

        public Guid QuoteId { get; set; }
        public int QuoteIdIdentity { get; init; }
        [Required(ErrorMessage = "*")]
        public string Name { get; init; }
        public string Notes { get; init; }
        [Required]
        public DateTime? DueDate { get; init; }
        public QuoteType Type { get; set; }
        public QuoteSubType SubType { get; set; }
        public IList<QuoteRequestEvent> Events { get; }
        public User Owner { get; set; }
        [Required]
        [Range( 2, Int32.MaxValue)]
        public int Attendees { get; init; }
        public override string ToString()
        {
            return $"{Name} - Events {Events.Count} - Attendees {Attendees}";
        }
    }
}