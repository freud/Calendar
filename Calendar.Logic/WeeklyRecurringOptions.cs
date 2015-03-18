namespace Calendar.Logic
{
    public class WeeklyRecurringOptions : RecurringOptions
    {
        public WeeklyRecurringOptions()
        {
            RepeatEvery = 1;
        }

        public WeekDays WeekDays { get; set; }
        public int RepeatEvery { get; set; }
    }
}