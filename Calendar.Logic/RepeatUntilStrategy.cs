using System;
using System.Collections.Generic;

namespace Calendar.Logic
{
    public abstract class RepeatUntilStrategy
    {
        public abstract bool CanBeRepeated(ICollection<Event> alreadyPopulatedEvents, DateTime newStartDate);
    }
}