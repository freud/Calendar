using Calendar.Data;
using Calendar.Logic;
using FluentAssertions;
using NUnit.Framework;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendar.Services.Tests
{
    [TestFixture]
    public class EventsServiceTests
    {
        private static DateTime _currentFakedTime = DateTime.Now;

        [Test]
        public void GetEvents_GetAnyListOfEvents_ReturnsListOfEvents()
        {
            // Arrange
            var eventsService = CreateDefaultService();

            // Act
            var listOfEvents = eventsService.GetEvents(_currentFakedTime, _currentFakedTime);

            // Assert
            listOfEvents.Should().NotBeNull();
        }

        [Test]
        public void GetEvents_GetNonEmptyListOfEvents_ReturnsNonEmptyList()
        {
            // Arrange
            var eventsService = CreateDefaultService();

            // Act
            var listOfEvents = eventsService.GetEvents(_currentFakedTime, _currentFakedTime);

            // Assert
            listOfEvents.Should().NotBeEmpty();
        }

        private IEnumerable<TestCaseData> GetEventsFromTimeRangeParameters
        {
            get
            {
                yield return new TestCaseData(_currentFakedTime.Date.AddDays(-2), _currentFakedTime.Date.AddDays(-1), new[] 
                {
                    new Event(_currentFakedTime.Date.AddDays(-1))
                });

                yield return new TestCaseData(_currentFakedTime.Date.AddDays(-1), _currentFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentFakedTime.Date)
                });

                yield return new TestCaseData(_currentFakedTime.Date.AddDays(-5), _currentFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentFakedTime.Date),
                    new Event(_currentFakedTime.Date.AddDays(10))
                });

                yield return new TestCaseData(_currentFakedTime.Date.AddDays(-5), _currentFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentFakedTime.Date.AddDays(-5)),
                    new Event(_currentFakedTime.Date.AddDays(1))
                });

                yield return new TestCaseData(_currentFakedTime.Date.AddDays(-5), _currentFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentFakedTime.Date.AddDays(-8)),
                    new Event(_currentFakedTime.Date.AddDays(1))
                });

                yield return new TestCaseData(_currentFakedTime.Date.AddDays(-10), _currentFakedTime.Date.AddDays(100), new[] 
                {
                    new Event(_currentFakedTime.Date.AddDays(-8)),
                    new Event(_currentFakedTime.Date.AddDays(1))
                });

            }
        }
        
        [TestCaseSource("GetEventsFromTimeRangeParameters")]
        public void GetEvents_GetEventsFromTimeRange_ReturnsEventsFromSpecifiedTimeRange(DateTime from, DateTime to, Event[] fakedEvents)
        {
            // Arrange
            var store = PrepareFakedEvents(fakedEvents);
            var eventsService = new EventsService(store);

            // Act
            var listOfEvents = eventsService.GetEvents(from, to).ToList();

            // Assert
            listOfEvents.Should().NotBeEmpty();
            listOfEvents.All(@event => @event.StartDate >= from && @event.StartDate <= to).Should().BeTrue();
        }

        [Test]
        public void GetEvents_GetEventsFromOutOfTimeRange_ReturnsEmptyListOfEvents()
        {
            // Arrange
            var store = PrepareFakedEvents(new []
            {
                new Event(_currentFakedTime.Date.AddDays(-2)),
                new Event(_currentFakedTime.Date.AddDays(2))
            });
            var eventsService = new EventsService(store);

            // Act
            var listOfEvents = eventsService.GetEvents(_currentFakedTime.Date.AddDays(-1), _currentFakedTime.Date.AddDays(1)).ToList();

            // Assert
            listOfEvents.Should().BeEmpty();
        }

        private IEventsService CreateDefaultService()
        {
            var store = PrepareFakedEvents(new[]
            {
                new Event(_currentFakedTime)
            });

            return new EventsService(store);
        }

        private IDocumentStore PrepareFakedEvents(Event[] fakedEvents)
        {
            var store = DbHelper.CreateStructures();
            using (var session = store.OpenSession())
            {
                foreach (var @event in fakedEvents)
                {
                    session.Store(@event);
                }
                session.SaveChanges();
            }
            return store;
        }
    }
}
