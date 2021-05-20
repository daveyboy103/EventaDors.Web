using System;
using System.ComponentModel.DataAnnotations;

namespace EventaDors.Entities.Classes
{
    public class Event
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Event name is required")]
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}