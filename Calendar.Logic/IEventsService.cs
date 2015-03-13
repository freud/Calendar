using System;
using System.Collections.Generic;

namespace Calendar.Logic
{
    public interface IEventsService
    {
        IEnumerable<Event> GetEvents(DateTime from, DateTime to);
    }
}