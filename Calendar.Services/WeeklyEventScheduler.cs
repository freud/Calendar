using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public class WeeklyEventScheduler
    {
        public List<Event> Populate(Event @event, DateTime @from, DateTime to)
        {
            if (!(@event.RecurringOptions is WeeklyRecurringOptions))
            {
                throw new InvalidOperationException("WeeklyEventScheduler requires WeeklyRecurringOptions as Event.ReccuringOptions");
            }

            if (@event.StartDate > from)
            {
                from = @event.StartDate;
            }

            var options = (WeeklyRecurringOptions) @event.RecurringOptions;
            var populatedEvents = new List<Event>();

            var duration = @event.EndDate - @event.StartDate;
            var totalDays = ((int) (to - @from).TotalDays) + 1;

            for (var i = 0; i < totalDays; i++)
            {
                var newStartDate = @from.AddDays(i) + @event.StartDate.TimeOfDay;
                var weekDay = ConvertDayOfWeek(newStartDate.DayOfWeek);
                var weekNumber = (int)Math.Floor(i / 7d);

                if (weekNumber % options.RepeatEvery == 0 && options.WeekDays.HasFlag(weekDay) && options.RepeatUntil.CanBeRepeated(populatedEvents, newStartDate))
                {
                    populatedEvents.Add(new Event(newStartDate, newStartDate + duration));
                }
            }

            return populatedEvents;
        }

        private WeekDays ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            return (WeekDays) Enum.Parse(typeof (WeekDays), dayOfWeek.ToString());
        }

    }
}