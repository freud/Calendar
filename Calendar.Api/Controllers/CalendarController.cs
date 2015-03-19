using System;
using System.Collections.Generic;
using System.Globalization;
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

        public IEnumerable<Event> GetEvents(DateTime @from, DateTime to, string timeZoneId)
        {
            return _eventsService.GetEvents(from, to, timeZoneId);
        }

        public IEnumerable<Event> GetEventsByYear(int year, string timeZoneId)
        {
            var from = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var to = new DateTime(year, 12, 31, 0, 0, 0, DateTimeKind.Unspecified);
            return _eventsService.GetEvents(from, to, timeZoneId);
        }

        public IEnumerable<Event> GetEventsByMonth(int year, int month, string timeZoneId)
        {
            var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var numberOfDaysInMonth = DateTime.DaysInMonth(year, month);
            var to = new DateTime(year, month, numberOfDaysInMonth, 0, 0, 0, DateTimeKind.Unspecified);
            return _eventsService.GetEvents(from, to, timeZoneId);
        }

        public IEnumerable<Event> GetEventsByWeek(int year, int week, string timeZoneId)
        {
            DateTime from;
            DateTime to;
            var dayInGivenWeek = DateTimeFormatInfo.CurrentInfo.Calendar.AddWeeks(new DateTime(year, 1, 1), week);

            if (week == 0)
            {
                from = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
                to = from.AddDays(7);
            }
            else if (week == GetWeeksInYear(year))
            {
                var firstDayOfWeek = dayInGivenWeek.AddDays((int)dayInGivenWeek.DayOfWeek + 1);
                from = new DateTime(year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0, DateTimeKind.Unspecified);
                to = new DateTime(year, 12, 31, 0, 0, 0, DateTimeKind.Unspecified);
            }
            else
            {
                var firstDayOfWeek = dayInGivenWeek.AddDays((int)dayInGivenWeek.DayOfWeek + 1);
                from = new DateTime(year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0, DateTimeKind.Unspecified);
                to = from.AddDays(7);
            }

            return _eventsService.GetEvents(from, to, timeZoneId);
        }

        public IEnumerable<Event> GetEventsByDay(int year, int month, int day, string timeZoneId)
        {
            var from = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
            var to = from.AddDays(1);
            return _eventsService.GetEvents(from, to, timeZoneId);
        }

        private int GetWeeksInYear(int year)
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var lastDayOfYear = new DateTime(year, 12, 31);

            return dfi.Calendar.GetWeekOfYear(lastDayOfYear, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
    }
}
