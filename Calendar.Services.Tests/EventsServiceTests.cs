using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar.Logic;
using FluentAssertions;
using NUnit.Framework;

namespace Calendar.Services.Tests
{
    [TestFixture]
    public class EventsServiceTests
    {
        [Test]
        public void GetEvents_GetAnyListOfEvents_ReturnsListOfEvents()
        {
            // Arrange
            var eventsService = CreateDefaultService();

            // Act
            var listOfEvents = eventsService.GetEvents(DateTime.Now, DateTime.Now);

            // Assert
            listOfEvents.Should().NotBeNull();
        }

        [Test]
        public void GetEvents_GetNonEmptyListOfEvents_ReturnsNonEmptyList()
        {
            // Arrange
            var eventsService = CreateDefaultService();

            // Act
            var listOfEvents = eventsService.GetEvents(DateTime.Now, DateTime.Now);

            // Assert
            listOfEvents.Should().NotBeEmpty();
        }

        private IEnumerable<TestCaseData> GetEventsFromTimeRangeParameters
        {
            get
            {
                yield return new TestCaseData(DateTime.Now.Date.AddDays(-2), DateTime.Now.Date.AddDays(-1), new[] 
                {
                    new Event(DateTime.Now.Date.AddDays(-1))
                });

                yield return new TestCaseData(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddDays(1), new[] 
                {
                    new Event(DateTime.Now.Date)
                });

                yield return new TestCaseData(DateTime.Now.Date.AddDays(-5), DateTime.Now.Date.AddDays(1), new[] 
                {
                    new Event(DateTime.Now.Date),
                    new Event(DateTime.Now.Date.AddDays(10))
                });

                yield return new TestCaseData(DateTime.Now.Date.AddDays(-5), DateTime.Now.Date.AddDays(1), new[] 
                {
                    new Event(DateTime.Now.Date.AddDays(-5)),
                    new Event(DateTime.Now.Date.AddDays(1))
                });

                yield return new TestCaseData(DateTime.Now.Date.AddDays(-5), DateTime.Now.Date.AddDays(1), new[] 
                {
                    new Event(DateTime.Now.Date.AddDays(-8)),
                    new Event(DateTime.Now.Date.AddDays(1))
                });
            }
        }

        [TestCaseSource("GetEventsFromTimeRangeParameters")]
        public void GetEvents_GetEventsFromTimeRange_ReturnsEventsFromSpecifiedTimeRange(DateTime from, DateTime to, Event[] fakedEvents)
        {
            // Arrange
            var eventsService = new EventsService(fakedEvents);

            // Act
            var listOfEvents = eventsService.GetEvents(from, to);

            // Assert
            listOfEvents.All(@event => @event.StartDate >= from && @event.StartDate <= to).Should().BeTrue();
        }

        private static IEventsService CreateDefaultService()
        {
            return new EventsService(new[]
            {
                new Event(DateTime.Now)
            });
        }
    }
}
