using System;
using System.Collections.Generic;
using Calendar.Helpers;
using Calendar.Logic;
using FluentAssertions;
using NUnit.Framework;

namespace Calendar.Services.Tests
{
    [TestFixture]
    public class EventSchedulerServiceTests
    {
        private IEnumerable<TestCaseData> PopulatesEventsEverySpecifiedDayParams
        {
            get
            {
                yield return new TestCaseData(WeekDays.Monday, new DateTime(2015, 3, 16));
                yield return new TestCaseData(WeekDays.Tuesday, new DateTime(2015, 3, 17));
                yield return new TestCaseData(WeekDays.Wednesday, new DateTime(2015, 3, 18));
                yield return new TestCaseData(WeekDays.Thursday, new DateTime(2015, 3, 19));
                yield return new TestCaseData(WeekDays.Friday, new DateTime(2015, 3, 20));
                yield return new TestCaseData(WeekDays.Saturday, new DateTime(2015, 3, 21));
                yield return new TestCaseData(WeekDays.Sunday, new DateTime(2015, 3, 22));
            }
        }

        [TestCaseSource("PopulatesEventsEverySpecifiedDayParams")]
        public void WeeklyEventScheduler_PopulatesEventsEverySpecifiedDay_ReturnsPopulatedEvents(WeekDays weekDays, DateTime expectedEventStartDate)
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 22);

            var eventDate = new DateTime(2015, 3, 17);
            
            var scheduler = new WeeklyEventScheduler();

            var options = new WeeklyRecurringOptions();
            options.WeekDays = weekDays;

            var @event = new Event(eventDate, eventDate.AddHours(1));
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Count.Should().Be(1);

            var result = populatedEvents[0];
            var eventDuration = (result.EndDate - result.StartDate);

            result.StartDate.Should().Be(expectedEventStartDate);
            eventDuration.Should().Be(TimeSpan.FromHours(1));
        }

        private IEnumerable<TestCaseData> PopulatesEventsEveryMondayInSpecifiedRangeParams
        {
            get
            {
                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 23), 2);
                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 28), 2);
                yield return new TestCaseData(new DateTime(2015, 3, 15), new DateTime(2015, 3, 28), 2);
                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 31), 2);
                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 30), 2);
            }
        }

        [TestCaseSource("PopulatesEventsEveryMondayInSpecifiedRangeParams")]
        public void WeeklyEventScheduler_PopulatesEventsEveryMondayInSpecifiedRange_ReturnsPopulatedEvents(DateTime calendarRangeFrom, DateTime calendarRangeTo, int expectedNumberOfMondays)
        {
            // Arrange
            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;

            var @event = new Event(new DateTime(2015, 3, 17), new DateTime(2015, 3, 18));
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Count.Should().Be(expectedNumberOfMondays);
            foreach (var @populatedEvent in populatedEvents)
            {
                var eventDuration = (@populatedEvent.EndDate - @populatedEvent.StartDate);
                eventDuration.Should().Be(TimeSpan.FromDays(1));

                @populatedEvent.StartDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
            }
        }
    }
}