using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar.Logic;
using Simple.Data;

namespace Calendar.Services
{
    public class EventsService : IEventsService
    {
        private readonly Event[] _events;

        public EventsService(Event[] events)
        {
            _events = events;
        }

        public IEnumerable<Event> GetEvents(DateTime @from, DateTime to)
        {
            return _events.Where(e => e.StartDate >= from && e.StartDate <= to);
        }
    }
}
