using System;
using System.Collections.Generic;

namespace Calendar.Logic
{
    public class RepeatXTimes : RepeatUntilStrategy
    {
        private readonly int _numberOfTimes;

        public RepeatXTimes(int numberOfTimes)
        {
            _numberOfTimes = numberOfTimes;
        }

        public override bool CanBeRepeated(ICollection<Event> alreadyPopulatedEvents, DateTime newStartDate)
        {
            return alreadyPopulatedEvents.Count < _numberOfTimes;
        }
    }
}