using System;
using System.Collections.Generic;

namespace Calendar.Logic
{
    public class RepeatUntilDate : RepeatUntilStrategy
    {
        private readonly DateTime _endDate;

        public RepeatUntilDate(DateTime endDate)
        {
            _endDate = endDate;
        }

        public override bool CanBeRepeated(ICollection<Event> alreadyPopulatedEvents, DateTime newStartDate)
        {
            return newStartDate <= _endDate;
        }
    }
}