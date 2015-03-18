using Calendar.Logic;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
        public void WeeklyEventScheduler_PopulatesEventsEverySpecifiedWeekDay_ReturnsPopulatedEvents(WeekDays weekDays, DateTime expectedEventStartDate)
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 22);
            var eventStartDate = new DateTime(2015, 3, 15);
            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = weekDays;

            var @event = new Event(eventStartDate, eventStartDate.AddHours(1));
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

        [Test]
        public void WeeklyEventScheduler_PopulateOutOfRangeEventEveryMonday_ReturnsEmptyList()
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 22);

            var eventStartDate = new DateTime(2015, 3, 17);

            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;

            var @event = new Event(eventStartDate, eventStartDate.AddHours(1));
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Count.Should().Be(0);
        }

        [Test]
        public void WeeklyEventScheduler_PopulatesOutOfRangeEvent_ReturnsEmptyList()
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 22);
            var eventStartDate = new DateTime(2015, 3, 23);
            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;

            var @event = new Event(eventStartDate, eventStartDate.AddHours(1));
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Count.Should().Be(0);
        }

        private IEnumerable<TestCaseData> PopulatesEventsEveryMondayInSpecifiedRangeParams
        {
            get
            {
                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 23), new[]
                {
                    new Event(new DateTime(2015, 3, 16), new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 23), new DateTime(2015, 3, 24))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 28), new[]
                {
                    new Event(new DateTime(2015, 3, 16), new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 23), new DateTime(2015, 3, 24))
                });
                yield return new TestCaseData(new DateTime(2015, 3, 15), new DateTime(2015, 3, 28), new[]
                {
                    new Event(new DateTime(2015, 3, 16), new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 23), new DateTime(2015, 3, 24))
                });
                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 31), new[]
                {
                    new Event(new DateTime(2015, 3, 23), new DateTime(2015, 3, 24)),
                    new Event(new DateTime(2015, 3, 30), new DateTime(2015, 3, 31))
                });
                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 30), new[]
                {
                    new Event(new DateTime(2015, 3, 23), new DateTime(2015, 3, 24)),
                    new Event(new DateTime(2015, 3, 30), new DateTime(2015, 3, 31))
                });
            }
        }

        [TestCaseSource("PopulatesEventsEveryMondayInSpecifiedRangeParams")]
        public void WeeklyEventScheduler_PopulatesEventsEveryMondayInSpecifiedRange_ReturnsPopulatedEvents(DateTime calendarRangeFrom, DateTime calendarRangeTo, Event[] expectedPopulatedEvents)
        {
            // Arrange
            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;

            var @event = new Event(new DateTime(2015, 3, 15), new DateTime(2015, 3, 16));
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Should().BeEquivalentTo(expectedPopulatedEvents);
        }

        private IEnumerable<TestCaseData> PopulatesEventsOnSpecifiedWeekdaysParams
        {
            get
            {
                yield return new TestCaseData(WeekDays.Monday, new Event[]{});
                yield return new TestCaseData(WeekDays.Monday | WeekDays.Tuesday, new[]
                {
                    new Event(new DateTime(2015, 3, 17))
                });
                yield return new TestCaseData(WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday, new[]
                {
                    new Event(new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 18))
                });
                yield return new TestCaseData(WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday | WeekDays.Thursday, new[]
                {
                    new Event(new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 18)),
                    new Event(new DateTime(2015, 3, 19))
                });
                yield return new TestCaseData(WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday | WeekDays.Thursday | WeekDays.Friday, new[]
                {
                    new Event(new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 18)),
                    new Event(new DateTime(2015, 3, 19)),
                    new Event(new DateTime(2015, 3, 20))
                });
                yield return new TestCaseData(WeekDays.Wednesday | WeekDays.Thursday | WeekDays.Friday, new[]
                {
                    new Event(new DateTime(2015, 3, 18)),
                    new Event(new DateTime(2015, 3, 19)),
                    new Event(new DateTime(2015, 3, 20))
                });
                yield return new TestCaseData(WeekDays.Friday, new[]
                {
                    new Event(new DateTime(2015, 3, 20))
                });
            }
        }

        [TestCaseSource("PopulatesEventsOnSpecifiedWeekdaysParams")]
        public void WeeklyEventScheduler_PopulatesEventsOnSpecifiedWeekdays_ReturnsPoplatedEvents(WeekDays weekDays, Event[] expectedPopulatedEvents)
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 22);
            var eventStartDate = new DateTime(2015, 3, 17);

            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = weekDays;

            var @event = new Event(eventStartDate);
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Should().BeEquivalentTo(expectedPopulatedEvents);
        }

        private IEnumerable<TestCaseData> PopulatesEventsOnSpecifiedWeekdaysInSpecifiedRangeParams
        {
            get
            {
                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 22), WeekDays.Monday, new Event[] {});

                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 30), WeekDays.Monday | WeekDays.Tuesday, new[] 
                {
                    new Event(new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 23)),
                    new Event(new DateTime(2015, 3, 24)),
                    new Event(new DateTime(2015, 3, 30))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 30), WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday, new[] 
                {
                    new Event(new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 18)),
                    new Event(new DateTime(2015, 3, 23)),
                    new Event(new DateTime(2015, 3, 24)),
                    new Event(new DateTime(2015, 3, 25)),
                    new Event(new DateTime(2015, 3, 30))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 3, 30), WeekDays.Monday | WeekDays.Tuesday | WeekDays.Friday, new[] 
                {
                    new Event(new DateTime(2015, 3, 17)),
                    new Event(new DateTime(2015, 3, 20)),
                    new Event(new DateTime(2015, 3, 23)),
                    new Event(new DateTime(2015, 3, 24)),
                    new Event(new DateTime(2015, 3, 27)),
                    new Event(new DateTime(2015, 3, 30))
                });
                yield return new TestCaseData(new DateTime(2015, 3, 17), new DateTime(2015, 4, 10), WeekDays.Thursday, new[] 
                {
                    new Event(new DateTime(2015, 3, 19)),
                    new Event(new DateTime(2015, 3, 26)),
                    new Event(new DateTime(2015, 4, 2)),
                    new Event(new DateTime(2015, 4, 9))
                });

            }
        }

        [TestCaseSource("PopulatesEventsOnSpecifiedWeekdaysInSpecifiedRangeParams")]
        public void WeeklyEventScheduler_PopulatesEventsOnSpecifiedWeekdaysInSpecifiedRange_ReturnsPoplatedEvents(DateTime calendarRangeFrom, DateTime calendarRangeTo, WeekDays weekDays, Event[] expectedPopulatedEvents)
        {
            // Arrange
            var eventStartDate = new DateTime(2015, 3, 17);

            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = weekDays;

            var @event = new Event(eventStartDate);
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Should().BeEquivalentTo(expectedPopulatedEvents);
        }

        private IEnumerable<TestCaseData> PopulatesEventsInSpecifiedRangeEveryTwoWeeksParams
        {
            get
            {
                yield return new TestCaseData(new DateTime(2015, 3, 18), new DateTime(2015, 3, 22), 2, new Event[] {});

                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 29), 2, new[] 
                {
                    new Event(new DateTime(2015, 3, 16))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 5, 29), 2, new[] 
                {
                    new Event(new DateTime(2015, 3, 16)),
                    new Event(new DateTime(2015, 3, 30)),
                    new Event(new DateTime(2015, 4, 13)),
                    new Event(new DateTime(2015, 4, 27)),
                    new Event(new DateTime(2015, 5, 11)),
                    new Event(new DateTime(2015, 5, 25))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 3, 29), 3, new[] 
                {
                    new Event(new DateTime(2015, 3, 16))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 16), new DateTime(2015, 5, 29), 3, new[] 
                {
                    new Event(new DateTime(2015, 3, 16)),
                    new Event(new DateTime(2015, 4, 6)),
                    new Event(new DateTime(2015, 4, 27)),
                    new Event(new DateTime(2015, 5, 18))
                });
            }
        }

        [TestCaseSource("PopulatesEventsInSpecifiedRangeEveryTwoWeeksParams")]
        public void WeeklyEventScheduler_PopulatesEventsInSpecifiedRangeOnMondayEverySpecifiedWeeks_ReturnsPopulatedEvents(DateTime calendarRangeFrom, DateTime calendarRangeTo, int repeatEvery, Event[] expectedPopulatedEvents)
        {
            // Arrange
            var eventStartDate = new DateTime(2015, 3, 15);
            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;
            options.RepeatEvery = repeatEvery;

            var @event = new Event(eventStartDate);
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Should().BeEquivalentTo(expectedPopulatedEvents);
        }

        private IEnumerable<TestCaseData> PopulatesEventsUntilSpecifiedDateParams
        {
            get
            {
                yield return new TestCaseData(new DateTime(2015, 3, 24), new[]
                {
                    new Event(new DateTime(2015, 3, 16)),
                    new Event(new DateTime(2015, 3, 23))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 16), new[]
                {
                    new Event(new DateTime(2015, 3, 16))
                });

                yield return new TestCaseData(new DateTime(2015, 4, 1), new[]
                {
                    new Event(new DateTime(2015, 3, 16)),
                    new Event(new DateTime(2015, 3, 23)),
                    new Event(new DateTime(2015, 3, 30))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 30), new[]
                {
                    new Event(new DateTime(2015, 3, 16)),
                    new Event(new DateTime(2015, 3, 23)),
                    new Event(new DateTime(2015, 3, 30))
                });

                yield return new TestCaseData(new DateTime(2015, 3, 15), new Event[] {});
            } 
        }
        
        [TestCaseSource("PopulatesEventsUntilSpecifiedDateParams")]
        public void WeeklyEventScheduler_PopulatesEventsUntilSpecifiedDate_ReturnsPopulatedEvents(DateTime repeatUntilDate, Event[] expectedPopulatedEvents)
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 31);
            var eventStartDate = new DateTime(2015, 3, 15);

            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;
            options.RepeatUntil = new RepeatUntilDate(repeatUntilDate);

            var @event = new Event(eventStartDate);
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Should().BeEquivalentTo(expectedPopulatedEvents);
        }

        private IEnumerable<TestCaseData> PopulatesEventsOnlyXTimesParams
        {
            get
            {
                yield return new TestCaseData(2, new[]
                {
                    new Event(new DateTime(2015, 3, 16)),
                    new Event(new DateTime(2015, 3, 23))
                });

                yield return new TestCaseData(1, new[]
                {
                    new Event(new DateTime(2015, 3, 16))
                });

                //yield return new TestCaseData(3, new[]
                //{
                //    new Event(new DateTime(2015, 3, 16)),
                //    new Event(new DateTime(2015, 3, 23)),
                //    new Event(new DateTime(2015, 3, 30))
                //});
            }
        }


        [TestCaseSource("PopulatesEventsOnlyXTimesParams")]
        public void WeeklyEventScheduler_PopulatesEventsOnlyXTimes_ReturnsPopulatedEvents(int numberOfTimes, Event[] expectedPopulatedEvents)
        {
            // Arrange
            var calendarRangeFrom = new DateTime(2015, 3, 16);
            var calendarRangeTo = new DateTime(2015, 3, 31);
            var eventStartDate = new DateTime(2015, 3, 15);

            var scheduler = new WeeklyEventScheduler();
            var options = new WeeklyRecurringOptions();
            options.WeekDays = WeekDays.Monday;
            options.RepeatUntil = new RepeatXTimes(numberOfTimes);

            var @event = new Event(eventStartDate);
            @event.RecurringOptions = options;

            // Act
            var populatedEvents = scheduler.Populate(@event, calendarRangeFrom, calendarRangeTo);

            // Assert
            populatedEvents.Should().BeEquivalentTo(expectedPopulatedEvents);
        }

    }
}