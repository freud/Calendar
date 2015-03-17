using System;
using Calendar.Helpers;

namespace Calendar.Logic
{
    public class Event
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RecurringOptions RecurringOptions { get; set; }

        public bool IsFullDay
        {
            get
            {
                return StartDate.TimeOfDay == TimeSpan.Zero && 
                       EndDate.TimeOfDay == TimeSpan.Zero;
            }
        }

        public bool IsRecurring
        {
            get { return RecurringOptions != null; }
        }

        /// <summary>
        /// Creates default
        /// </summary>
        public Event()
        {
            StartDate = ApplicationTime.Current;
            EndDate = StartDate.AddHours(1);
        }

        public Event(DateTime startDate) : this(startDate, startDate.AddHours(1))
        {
        }

        public Event(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
