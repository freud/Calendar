using System;
using System.Diagnostics;
using Calendar.Helpers;

namespace Calendar.Logic
{
    [DebuggerDisplay("{StartDate} - {EndDate}")]
    public class Event : IEquatable<Event>
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

        #region IEquatable

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Event)) return false;
            return Equals((Event) obj);
        }

        public bool Equals(Event other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate) && Equals(RecurringOptions, other.RecurringOptions);
        }

        #endregion
    }
}
