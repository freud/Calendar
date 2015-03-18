using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public class WeeklyEventScheduler
    {
        public List<Event> Populate(Event @event, DateTime @from, DateTime to)
        {
            if (@event.StartDate > from)
            {
                from = @event.StartDate;
            }

            var options = (WeeklyRecurringOptions) @event.RecurringOptions;
            var populatedEvents = new List<Event>();

            foreach (WeekDays singleWeekDay in Enum.GetValues(typeof (WeekDays)))
            {
                if (!options.WeekDays.HasFlag(singleWeekDay))
                {
                    continue;
                }

                var repeatEveryMultiplication = 7*options.RepeatEvery;
                var firstOccurenceDate = GetFirstOccurenceDate(@from, singleWeekDay);
                var duration = @event.EndDate - @event.StartDate;
                var numberOfOccurences = (int)Math.Floor((to - firstOccurenceDate).TotalDays / repeatEveryMultiplication) + 1;

                for (var i = 0; i < numberOfOccurences; i++)
                {
                    var newStartDate = firstOccurenceDate.AddDays(repeatEveryMultiplication * i);
                    var newEndDate = newStartDate + duration;

                    if (options.RepeatUntil == null || newStartDate <= options.RepeatUntil.EndDate)
                    {
                        populatedEvents.Add(new Event(newStartDate, newEndDate));
                    }
                }
            }

            return populatedEvents;
        }

        /// <summary>
        /// Helper method to calculate first specified week day since given date
        /// </summary>
        /// <param name="from">date date</param>
        /// <param name="singleWeekDay">week day to find</param>
        /// <remarks>This is internally used by Populate</remarks>
        public DateTime GetFirstOccurenceDate(DateTime @from, WeekDays singleWeekDay)
        {
            var weekDayOffset = ((int)Math.Log((int)singleWeekDay, 2));
            var fromWeekDayOffset = NormalizeDayOfWeekOffset(@from);

            if (weekDayOffset == fromWeekDayOffset)
            {
                return @from;
            }

            if (weekDayOffset < fromWeekDayOffset)
            {
                weekDayOffset += 7 - fromWeekDayOffset;
            }
            else
            {
                return @from.AddDays(weekDayOffset - fromWeekDayOffset);    
            }

            return @from.AddDays(weekDayOffset);
        }

        /// <summary>
        /// Normalizes DayOfWeek to a 0-based offset where Monday becomes first value (0).
        /// </summary>
        /// <param name="date">date to normalize</param>
        private int NormalizeDayOfWeekOffset(DateTime date)
        {
            var fromWeekDayOffset = ((int) (date.DayOfWeek - 1));
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                fromWeekDayOffset = 6;
            }

            return fromWeekDayOffset;
        }
    }
}