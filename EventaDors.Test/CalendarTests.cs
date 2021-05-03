using System;
using System.Collections.Generic;
using System.Linq;
using EventaDors.DataManagement;
using EventaDors.DataManagement.Processors;
using NUnit.Framework;

namespace EventaDors.Test
{
    [TestFixture]
    public class CalendarTests
    {
        [Test]
        public void ShouldTestForAvailability()
        {
            var checker = 
                new InternalAvailabilityChecker(
                    new Wrapper("Server=tcp:eventadors1.database.windows.net,1433;Initial Catalog=EventaDors;Persist Security Info=False;User ID=dharrin4;Password=AskB4ntRy;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            IList<DateTime> proposedDates = new List<DateTime>
            {
                new DateTime(2022, 11, 07),
                new DateTime(2022, 12, 12)
            };

            var ret = checker.CheckAvailability(proposedDates, 7);
            
            Assert.IsTrue(ret.Last().Available);
            Assert.IsFalse(ret.First().Available);
        }

        [Test]
        public void ShouldImportAvailabilityFromCsv()
        {
            var checker = 
                new InternalAvailabilityChecker(
                    new Wrapper("Server=tcp:eventadors1.database.windows.net,1433;Initial Catalog=EventaDors;Persist Security Info=False;User ID=dharrin4;Password=AskB4ntRy;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            var list = new List<string>
            {
                "12-Dec-2023, 31-Dec-2023",
                "12-Mar-2023, 31-Apr-2023",
                "12-Jun-2023, 21-Jun-2023",
            };

            checker.Import(list, 7);
        }
    }
}