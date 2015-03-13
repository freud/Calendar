using System;
using System.Collections.Generic;
using System.Linq;
using Calendar.Api.Controllers;
using Calendar.Logic;
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
            DateTime now = DateTime.Now;

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
            var from = DateTime.Now;
            var to = DateTime.Now.AddMinutes(1);

            var eventsService = Substitute.For<IEventsService>();
            
            var controller = new CalendarController(eventsService);

            // Act
            controller.GetEvents(@from, to);

            // Assert
            eventsService.Received(1).GetEvents(from, to);
        }

    }
}
