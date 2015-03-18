using System;

namespace Calendar.Logic
{
    public class RepeatUntilDate
    {
        public DateTime EndDate { get; set; }

        public RepeatUntilDate(DateTime endDate)
        {
            EndDate = endDate;
        }
    }
}