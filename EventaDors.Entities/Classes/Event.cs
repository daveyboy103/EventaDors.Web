using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventaDors.Entities.Classes
{
    public class Event
    {
        public Event()
        {
            SubTypes = new List<QuoteSubType>();
        }
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Event name is required")]
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public bool HasSubType(QuoteSubType subType)
        {
            foreach (var quoteSubType in SubTypes)
            {
                if (quoteSubType.Id == subType.Id)
                {
                    return true;
                }
            }

            return false;
        }
        public IList<QuoteSubType> SubTypes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}