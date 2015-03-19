using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public class OneTimeEventScheduler : IScheduler
    {
        public List<Event> Populate(Event @event, DateTime @from, DateTime to)
        {
            return new List<Event> { @event };
        }
    }
}