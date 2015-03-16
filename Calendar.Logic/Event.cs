using System;

namespace Calendar.Logic
{
    public class Event
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }

        public Event()
        {
        }

        public Event(DateTime startDate)
        {
            StartDate = startDate;
        }
    }
}
