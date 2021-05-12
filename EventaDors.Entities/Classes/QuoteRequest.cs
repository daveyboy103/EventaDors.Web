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
        public int QuoteIdIdentity { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        public string Notes { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        public QuoteType Type { get; set; }
        public QuoteSubType SubType { get; set; }
        public IList<QuoteRequestEvent> Events { get; }
        public User Owner { get; set; }
        [Required]
        [Range( 2, Int32.MaxValue)]
        public int Attendees { get; set; }

        public override string ToString()
        {
            return $"{Name} - Events {Events.Count} - Attendees {Attendees}";
        }
    }
}