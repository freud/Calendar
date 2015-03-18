using System;

namespace Calendar.Logic
{
    public class WeeklyRecurringOptions : RecurringOptions
    {
        public WeekDays WeekDays { get; set; }
        public int RepeatEvery { get; set; }

        private RepeatUntilStrategy _repeatUntil;
        public RepeatUntilStrategy RepeatUntil
        {
            get { return _repeatUntil; }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("RepeatUntil requires non-null value");
                }

                _repeatUntil = value;
            }
        }

        public WeeklyRecurringOptions()
        {
            RepeatEvery = 1;
            RepeatUntil = new RepeatAlways();
        }
    }
}