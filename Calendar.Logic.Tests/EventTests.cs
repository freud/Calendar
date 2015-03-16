using System;
using NUnit.Framework;
using FluentAssertions;

namespace Calendar.Logic.Tests
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void Event_EventCanBeTransformedToFullDayEvent_ReturnsTransformedFullDayEvent()
        {
            // Arrange
            var eventDate = new DateTime(2015, 1, 1, 3, 14, 15);
            var expectedFullDayEventDate = new DateTime(2015, 1, 1, 0, 0, 0);

            var @event = new Event(eventDate);
            var translator = new EventTranslator();

            // Act
            var fullDayEvent = translator.TranslateToFullDayEvent(@event);

            // Assert
            fullDayEvent.StartDate.Should().Be(expectedFullDayEventDate);
        }
    }
}