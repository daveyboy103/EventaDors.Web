using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EventaDors.Entities.Classes
{
    public class QuoteTemplate : CreatedModifiedBase
    {
        public QuoteTemplate()
        {
            Events = new List<QuoteTemplateEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        [Required]
        [Range( 2, Int32.MaxValue)]
        public int Attendees { get; set; }
        public QuoteType Type { get; set; }
        public QuoteSubType SubType { get; set; }
        public IList<QuoteTemplateEvent> Events { get; }
        public override string ToString()
        {
            return $"{Name} - {Events.Count} Events";
        }
    }
}