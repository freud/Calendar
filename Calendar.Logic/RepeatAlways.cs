using System;
using System.Collections.Generic;

namespace Calendar.Logic
{
    public class RepeatAlways : RepeatUntilStrategy
    {
        public override bool CanBeRepeated(ICollection<Event> alreadyPopulatedEvents, DateTime newStartDate)
        {
            return true;
        }
    }
}