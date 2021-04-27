using System;

namespace EventaDors.Entities.Classes
{
    public class Deadline
    {
        public int QuoteRequestElementId { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Weeks { get; set; }
        public string Status { get; set; }
    }
}