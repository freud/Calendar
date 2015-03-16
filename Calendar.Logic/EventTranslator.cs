namespace Calendar.Logic
{
    public class EventTranslator
    {
        public FullDayEvent TranslateToFullDayEvent(Event @event)
        {
            var fullDayEvent = new FullDayEvent
            {
                StartDate = @event.StartDate.Date
            };

            return fullDayEvent;
        }
    }
}