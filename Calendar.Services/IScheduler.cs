using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public interface IScheduler
    {
        List<Event> Populate(Event @event, DateTime @from, DateTime to);
    }
}