using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IAvailabilityCheck
    {
        IEnumerable<IAvailabilityResult> CheckAvailability(IEnumerable<DateTime> proposedDates, int userId);
        bool Import(string fileName, int userId);
        bool Import(IEnumerable<string> csv, int userId);
    }

    public interface IAvailabilityResult
    {
        DateTime ProposedDate { get; set; }
        bool Available { get; set; }
    }
}