using System;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class AvailabilityResult : IAvailabilityResult
    {
        public DateTime ProposedDate { get; set; }
        public bool Available { get; set; }
    }
}