using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventaDors.Entities.Classes
{
    public class Event : CreatedModifiedBase
    {
        public Event()
        {
            SubTypes = new List<QuoteSubType>();
        }
        
        public int Id { get; init; }
        [Required(ErrorMessage = "Event name is required")]
        public string Name { get; init; }
        public string Notes { get; init; }
        public string Link { get; init; }

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