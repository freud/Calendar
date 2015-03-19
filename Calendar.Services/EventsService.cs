using System;
using System.Collections.Generic;
using System.Linq;
using Calendar.Logic;
using Raven.Client;
using Raven.Client.Linq;

namespace Calendar.Services
{
    public class EventsService : IEventsService
    {
        private readonly IDocumentStore _store;
        private readonly IScheduler[] _schedulers;

        public EventsService(IDocumentStore store) : this(store, new[] {new OneTimeEventScheduler()})
        {
        }

        public EventsService(IDocumentStore store, IScheduler[] schedulers)
        {
            _store = store;
            _schedulers = schedulers;
        }

        public IEnumerable<Event> GetEvents(DateTime rangeFrom, DateTime rangeTo, string timeZoneInfo)
        {
            if (rangeFrom.Kind != DateTimeKind.Unspecified)
            {
                throw new ArgumentException("Range from time must be local time", "rangeFrom");
            }
            if (rangeTo.Kind != DateTimeKind.Unspecified)
            {
                throw new ArgumentException("Range to time must be local time", "rangeTo");
            }

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfo);
            if (!timeZone.Equals(TimeZoneInfo.Utc))
            {
                rangeFrom = TimeZoneInfo.ConvertTimeToUtc(rangeFrom, timeZone);
                rangeTo = TimeZoneInfo.ConvertTimeToUtc(rangeTo, timeZone);
            }
            
            using (var session = _store.OpenSession())
            {
                var events = session.Query<Event>().Where(e => e.StartDate >= rangeFrom && e.StartDate <= rangeTo).ToList();
                var resultEvents = new List<Event>();
                foreach (var @event in events)
                {
                    foreach (var scheduler in _schedulers)
                    {
                        resultEvents.AddRange(scheduler.Populate(@event, rangeFrom, rangeTo));
                    }
                }

                ModifyEventsToLocalTimeZone(resultEvents, timeZone);

                return resultEvents;
            }
        }

        private void ModifyEventsToLocalTimeZone(List<Event> events, TimeZoneInfo timeZone)
        {
            foreach (var @event in events)
            {
                @event.StartDate = TimeZoneInfo.ConvertTimeFromUtc(@event.StartDate, timeZone);
                @event.EndDate = TimeZoneInfo.ConvertTimeFromUtc(@event.EndDate, timeZone);
            }
        }
    }
}
