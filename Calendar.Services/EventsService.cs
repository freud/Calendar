using System;
using System.Collections.Generic;
using Calendar.Logic;
using Raven.Client;
using Raven.Client.Linq;

namespace Calendar.Services
{
    public class EventsService : IEventsService
    {
        private readonly IDocumentStore _store;

        public EventsService(IDocumentStore store)
        {
            _store = store;
        }

        public IEnumerable<Event> GetEvents(DateTime @from, DateTime to)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Event>().Where(e => e.StartDate >= @from && e.StartDate <= to);
            }
        }
    }
}
