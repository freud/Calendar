using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public class WeeklyEventScheduler
    {
        public List<Event> Populate(Event @event, DateTime @from, DateTime to)
        {
            var options = (WeeklyRecurringOptions) @event.RecurringOptions;

            var firstOccurenceDate = GetFirstOccurenceDate(@from, options);

            var duration = @event.EndDate - @event.StartDate;

            var populatedEvents = new List<Event>();
            var numberOfOccurences = (int)Math.Floor((to - firstOccurenceDate).TotalDays/7) + 1;
            for (var i = 0; i < numberOfOccurences; i++)
            {
                var newStartDate = firstOccurenceDate.AddDays(7*i);
                var newEndDate = newStartDate + duration;
                populatedEvents.Add(new Event(newStartDate, newEndDate));
            }

            return populatedEvents;
        }

        private DateTime GetFirstOccurenceDate(DateTime @from, WeeklyRecurringOptions options)
        {
            var weekDayOffset = ((int) Math.Log((int) options.WeekDays, 2));

            var fromWeekDayOffset = ((int) (@from.DayOfWeek - 1));
            if (@from.DayOfWeek == DayOfWeek.Sunday)
            {
                fromWeekDayOffset = 6; // Zero based week day
            }
            if (@from.DayOfWeek != DayOfWeek.Monday)
            {
                weekDayOffset += 7 - fromWeekDayOffset;
            }

            return @from.AddDays(weekDayOffset);
        }
    }
}