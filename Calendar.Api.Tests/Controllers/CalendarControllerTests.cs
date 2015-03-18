using System;
using System.Collections.Generic;
using Calendar.Api.Controllers;
using Calendar.Helpers;
using Calendar.Logic;
using Calendar.Services;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

namespace Calendar.Service.Tests.Controllers
{
    [TestFixture]
    public class CalendarControllerTests
    {
        [Test]
        public void GetEvents_GetAnyListOfEvents_ReturnsListOfEvents()
        {
            // Arrange
            DateTime now = ApplicationTime.Current;

            var fakeEvents = new List<Event>
            {
                new Event(now),
                new Event(now)
            };

            var eventsService = Substitute.For<IEventsService>();
            eventsService.GetEvents(now, now).Returns(fakeEvents);

            var controller = new CalendarController(eventsService);

            // Act
            var listOfEvents = controller.GetEvents(now, now);

            // Assert
            listOfEvents.ShouldAllBeEquivalentTo(fakeEvents);
        }

        [Test]
        public void GetEvents_GetEventsWithSpecifiedArguments_ChecksIfControllerCallsEventsServiceWithTheseArguments()
        {
            // Arrange
            var from = ApplicationTime.Current;
            var to = ApplicationTime.Current.AddMinutes(1);

            var eventsService = Substitute.For<IEventsService>();
            
            var controller = new CalendarController(eventsService);

            // Act
            controller.GetEvents(@from, to);

            // Assert
            eventsService.Received(1).GetEvents(from, to);
        }

    }
}
