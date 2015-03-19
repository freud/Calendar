using System;
using System.Collections.Generic;
using Calendar.Logic;

namespace Calendar.Services
{
    public interface IEventsService
    {
        /// <summary>
        /// Get events from specified range. Time range must be defined in local time zone.
        /// </summary>
        /// <param name="rangeFrom">local from time</param>
        /// <param name="rangeTo">local to time</param>
        /// <param name="timeZoneId">.NET Framework time zone identifier <see cref="System.TimeZoneInfo.Id"/></param>
        IEnumerable<Event> GetEvents(DateTime rangeFrom, DateTime rangeTo, string timeZoneId);
    }
}