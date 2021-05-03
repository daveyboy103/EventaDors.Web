using System;
using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.DataManagement.Processors
{
    public class InternalAvailabilityChecker : IAvailabilityCheck
    {
        private readonly Wrapper _wrapper;

        public InternalAvailabilityChecker(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public IEnumerable<IAvailabilityResult> CheckAvailability(IEnumerable<DateTime> proposedDates, int userId)
        {
            return _wrapper.CheckUserAvailability(proposedDates, userId);
        }

        public bool Import(string fileName, int userId)
        {
            throw new NotImplementedException();
        }

        public bool Import(IEnumerable<string> csv, int userId)
        {
            try
            {
                foreach (string item in csv)
                {
                    var dates = item.Split(',');

                    DateTime from;
                    DateTime to;

                    if (dates.Length < 2) continue;
                    if (DateTime.TryParse(dates[0], out from))
                    {
                        if (DateTime.TryParse(dates[1], out to))
                        {
                            _wrapper.AddNonAvailability(from, to, userId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}