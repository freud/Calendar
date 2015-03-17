using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public interface IEventsService
    {
        IEnumerable<Event> GetEvents(DateTime from, DateTime to);
    }
}