using System;
using Calendar.Helpers;
using NUnit.Framework;
using FluentAssertions;

namespace Calendar.Logic.Tests
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void Event_WhenInitializedWithDefaultCtor_ReturnsEventFromNowToOneHourLater()
        {
            // Arrange
            var startDate = new DateTime(2015, 1, 1, 1, 1, 1);
            var expectedEndDate = new DateTime(2015, 1, 1, 2, 1, 1);

            ApplicationTime._replaceCurrentTimeLogic(() => startDate);

            // Act
            var @event = new Event();

            // Assert
            @event.StartDate.Should().Be(startDate);
            @event.EndDate.Should().Be(expectedEndDate);
        }

        [Test]
        public void Event_WhenInitializedWithFullDayDates_IsFullDayReturnsTrue()
        {
            // Arrange
            var startDate = new DateTime(2015, 1, 1, 0, 0, 0);
            var endDate = new DateTime(2015, 1, 2, 0, 0, 0);

            // Act
            var @event = new Event(startDate, endDate);

            // Assert
            @event.IsFullDay.Should().BeTrue();
        }

        [Test]
        public void Event_WhenInitializedWithNonFullDayDates_IsFullDayReturnsFalse()
        {
            // Arrange
            var startDate = new DateTime(2015, 1, 1, 1, 2, 3);
            var endDate = new DateTime(2015, 1, 2, 4, 5, 6);

            // Act
            var @event = new Event(startDate, endDate);

            // Assert
            @event.IsFullDay.Should().BeFalse();
        }

        [Test]
        public void Event_WhenConstructedWithOnlyStartDate_ReturnsEventWithEndDateSetToOneHourAfterStartDate()
        {
            // Arrange
            var startDate = new DateTime(2015, 1, 1, 1, 1, 1);
            var expectedEndDate = new DateTime(2015, 1, 1, 2, 1, 1);

            // Act
            var @event = new Event(startDate);

            // Assert
            @event.EndDate.Should().Be(expectedEndDate);
        }

        [Test]
        public void Event_WhenInitializedWithExampleRecurringConfiguration_IsRecurringReturnsTrue()
        {
            // Arrange
            var startDate = new DateTime(2015, 1, 1, 1, 2, 3);
            var endDate = new DateTime(2015, 1, 2, 4, 5, 6);
            var @event = new Event(startDate, endDate);

            // Act
            @event.RecurringOptions = new RecurringOptions();

            // Assert
            @event.RecurringOptions.Should().NotBeNull();
            @event.IsRecurring.Should().BeTrue();
        }

        [Test]
        public void Event_WhenNotInitializedWithRecurringConfiguration_IsRecurringReturnsFalse()
        {
            // Arrange
            var startDate = new DateTime(2015, 1, 1, 1, 2, 3);
            var endDate = new DateTime(2015, 1, 2, 4, 5, 6);

            // Act
            var @event = new Event(startDate, endDate);

            // Assert
            @event.RecurringOptions.Should().BeNull();
            @event.IsRecurring.Should().BeFalse();
        }

        [Test]
        public void Event_EqualsWithTwoSameObjects_ReturnsTrue()
        {
            // Arrange
            var event1 = new Event(new DateTime(2015, 1, 1));
            var event2 = new Event(new DateTime(2015, 1, 1));

            // Act
            var result = event1.Equals(event2);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Event_EqualsWithTwoDifferentObjects_ReturnsFalse()
        {
            // Arrange
            var event1 = new Event(new DateTime(2015, 1, 1));
            var event2 = new Event(new DateTime(2015, 1, 2));

            // Act
            var result = event1.Equals(event2);

            // Assert
            result.Should().BeFalse();
        }
    }
}