using System;

namespace Calendar.Logic
{
    public class Event
    {
        public DateTime StartDate { get; set; }

        public Event(DateTime startDate)
        {
            StartDate = startDate;
        }
    }
}
