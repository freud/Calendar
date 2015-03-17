using System;
using System.Collections.Generic;
using System.Web.Http;
using Calendar.Logic;
using Calendar.Services;

namespace Calendar.Api.Controllers
{
    public class CalendarController : ApiController
    {
        private readonly IEventsService _eventsService;

        public CalendarController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        public IEnumerable<Event> GetEvents(DateTime from, DateTime to)
        {
            return _eventsService.GetEvents(from, to);
        }
    }
}
