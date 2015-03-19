using Calendar.Data;
using Calendar.Helpers;
using Calendar.Logic;
using FluentAssertions;
using NSubstitute;
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
        private static DateTime _currentUtcFakedTime = ApplicationTime.Current;
        private static DateTime _currentLocalFakedTime = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(_currentUtcFakedTime, TimeZoneInfo.Local), DateTimeKind.Unspecified);

        [Test]
        public void GetEvents_GetAnyListOfEvents_ReturnsListOfEvents()
        {
            // Arrange
            var eventsService = CreateDefaultService();

            // Act
            var listOfEvents = eventsService.GetEvents(_currentLocalFakedTime, _currentLocalFakedTime, "UTC");

            // Assert
            listOfEvents.Should().NotBeNull();
        }

        [Test]
        public void GetEvents_GivenLocalFromToParamsAndFakedEventWithUtcDates_ReturnsEventWithLocalDates()
        {
            // Arrange
            var eventsService = CreateDefaultService();
            var expectedEvent = new Event(_currentLocalFakedTime);

            // Act
            var listOfEvents = eventsService.GetEvents(_currentLocalFakedTime, _currentLocalFakedTime, TimeZoneInfo.Local.Id);

            // Assert
            listOfEvents.Count().Should().Be(1);
            listOfEvents.First().Should().Be(expectedEvent);
        }

        [Test]
        public void GetEvents_GetEmptyListOfEvents_ReturnsEmptyList()
        {
            // Arrange
            var store = PrepareFakedEvents(new Event[]{});
            var eventsService = (IEventsService) new EventsService(store);

            // Act
            var listOfEvents = eventsService.GetEvents(_currentLocalFakedTime, _currentLocalFakedTime, "UTC");

            // Assert
            listOfEvents.Should().BeEmpty();
        }

        private IEnumerable<TestCaseData> GetEventsFromTimeRangeParameters
        {
            get
            {
                yield return new TestCaseData(_currentLocalFakedTime.Date.AddDays(-2), _currentLocalFakedTime.Date.AddDays(-1), new[] 
                {
                    new Event(_currentLocalFakedTime.Date.AddDays(-1))
                });

                yield return new TestCaseData(_currentLocalFakedTime.Date.AddDays(-1), _currentLocalFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentLocalFakedTime.Date)
                });

                yield return new TestCaseData(_currentLocalFakedTime.Date.AddDays(-5), _currentLocalFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentLocalFakedTime.Date),
                    new Event(_currentLocalFakedTime.Date.AddDays(10))
                });

                yield return new TestCaseData(_currentLocalFakedTime.Date.AddDays(-5), _currentLocalFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentLocalFakedTime.Date.AddDays(-5)),
                    new Event(_currentLocalFakedTime.Date.AddDays(1))
                });

                yield return new TestCaseData(_currentLocalFakedTime.Date.AddDays(-5), _currentLocalFakedTime.Date.AddDays(1), new[] 
                {
                    new Event(_currentLocalFakedTime.Date.AddDays(-8)),
                    new Event(_currentLocalFakedTime.Date.AddDays(1))
                });

                yield return new TestCaseData(_currentLocalFakedTime.Date.AddDays(-10), _currentLocalFakedTime.Date.AddDays(100), new[] 
                {
                    new Event(_currentLocalFakedTime.Date.AddDays(-8)),
                    new Event(_currentLocalFakedTime.Date.AddDays(1))
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
            var listOfEvents = eventsService.GetEvents(@from, to, "UTC").ToList();

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
                new Event(_currentLocalFakedTime.Date.AddDays(-2)),
                new Event(_currentLocalFakedTime.Date.AddDays(2))
            });
            var eventsService = new EventsService(store);

            // Act
            var listOfEvents = eventsService.GetEvents(_currentLocalFakedTime.Date.AddDays(-1), _currentLocalFakedTime.Date.AddDays(1), "UTC").ToList();

            // Assert
            listOfEvents.Should().BeEmpty();
        }

        [Test]
        public void GetEvents_GivenSpecifiedTimeZone_ReturnsEventsWithDatesWithCorrectTimeZone()
        {
            // Arrange
            var timeZoneId = "Central European Standard Time";
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var utcEventStartDate = _currentUtcFakedTime.Date;
            var localEventStartDate = TimeZoneInfo.ConvertTimeFromUtc(utcEventStartDate, timeZoneInfo);

            var expectedEvents = new[]
            {
                new Event(localEventStartDate)
            };

            var store = PrepareFakedEvents(new[]
            {
                new Event(utcEventStartDate)
            });
            var eventsService = new EventsService(store);

            // Act
            var listOfEvents = eventsService.GetEvents(_currentLocalFakedTime.Date.AddDays(-1), _currentLocalFakedTime.Date.AddDays(1), timeZoneId).ToList();

            // Assert
            listOfEvents.Should().BeEquivalentTo(expectedEvents);
        }

        [Test]
        public void GetEvents_GiverSpecifiedWrongTimeZone_ThrowsInvalidOperationException()
        {
            // Arrange
            var timeZoneId = "Wrong time zone";
            var store = PrepareFakedEvents(new[]
            {
                new Event(DateTime.UtcNow)
            });
            var eventsService = new EventsService(store);

            // Act
            Action action = () => eventsService.GetEvents(_currentLocalFakedTime, _currentLocalFakedTime, timeZoneId);

            // Assert
            action.ShouldThrow<TimeZoneNotFoundException>();
        }

        [Test]
        public void GetEvents_GivenWrongUtcFromTime_ThrowsArgumentException()
        {
            // Arrange
            var timeZoneId = "Central European Standard Time";
            var store = PrepareFakedEvents(new Event[] { });
            var eventsService = new EventsService(store);

            // Act
            Action action = () => eventsService.GetEvents(DateTime.UtcNow, DateTime.Now, timeZoneId);

            // Assert
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void GetEvents_GivenWrongUtcToTime_ThrowsArgumentException()
        {
            // Arrange
            var timeZoneId = "Central European Standard Time";
            var store = PrepareFakedEvents(new Event[] { });
            var eventsService = new EventsService(store);

            // Act
            Action action = () => eventsService.GetEvents(DateTime.Now, DateTime.UtcNow, timeZoneId);

            // Assert
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void GetEvents_GivenFakedWeeklyScheduler_ChecksIfSchedulerIsCalledForGivenEventByService()
        {
            // Arrange
            var timeZoneId = "Central European Standard Time";
            var fakedEvent = new Event(_currentUtcFakedTime);
            fakedEvent.RecurringOptions = new WeeklyRecurringOptions();

            var scheduler = Substitute.For<IScheduler>();
            scheduler.Populate(Arg.Any<Event>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(new List<Event>());
            var store = PrepareFakedEvents(new [] { fakedEvent });
            var eventsService = new EventsService(store, new[] { scheduler });

            // Act
            eventsService.GetEvents(_currentLocalFakedTime, _currentLocalFakedTime, timeZoneId);

            // Assert
            scheduler.Received(1).Populate(Arg.Any<Event>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());
        }

        private IEventsService CreateDefaultService()
        {
            var store = PrepareFakedEvents(new[]
            {
                new Event(_currentUtcFakedTime)
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
