# Calendar
Calendar app as a result of some task based on the specified design document.

This is only training / examination app.

## Calendar features
Some features are based on the Google Calendar functionality.

Implemented functionalities:

- Scheduling events: one-time / full day / recurring on a weekly intervals

## Expected deliveries

### System architecture
System is based on the ASP.NET Web API.

### Suggested technologies
1. [ASP.NET Web Api](http://www.asp.net/web-api)
2. NoSQL database ([RavenDB](http://ravendb.net/)), 
3. MVVM UI framework (e.g. [AngularJS](https://angularjs.org/), [React](http://facebook.github.io/react/))

### Splitting system into applications / component possibilities
System would be split into components by using CQRS (by own implementation or/and by NServiceBus/Masstransit).

### Database type
For the application needs we provide RavenDb as NoSQL database, which gives us simple way of storing and querying all calendar events. 
It also makes testing simple with in-memory RavenDB adapter.

### Solution to the problem of creating, storing, displaying and editing event series
1. Events should be persisted in data store when user is creating new calendar event. 
When user fills the form for event and confirm it then proper action on the application side is being called. For example it could be a "CreateEvent" action method in the Web API controller.
This action should transform given form data (view model) into the Event and then should call "CreateEvent" at the "EventsService" which simply saves event directly into data store (RavenDB in this case).
UI could communicate with Web API RESTful service. Communication on the UI can be provided by JavaScript (e.g. AngularJS or other framework of this kind).

2. Changing (editing) event series (recurring event) is simply changing one event with recurring options (only one recurring event stored object - not series of objects). Event with recurring options (event series) represents a series of events when getting events to display. 
In other way: changing event series is simply editing one specific type of event (recurring event).

3. When single event is edited in a series of events then the application is creating new single event based on  specific options for event series (one event with recurring options) and at the same time the date of this single edited event is added to an exclusion list for related recurring event, so that this event series no longer generates an event for this specific date as it has now became an independent event.

### Getting list of data for different calendar views (year, month, week, day)
The example of getting list of events for year, month, week, day is provided in the "[Calendar.Api](https://github.com/freud/Calendar/tree/master/Calendar.Api)/[CalendarController](https://github.com/freud/Calendar/blob/master/Calendar.Api/Controllers/CalendarController.cs)" by the following methods:

1. **`GetEventsByYear`**
2. **`GetEventsByMonth`**
3. **`GetEventsByWeek`**
4. **`GetEventsByDay`**

Methods above are only wrappers for **`GetEvents`** method which can be used to get events in any specified date and time range.
Of course only these four method wrappers are reasonable from the practical and functional point of view. **`GetEvents`** method could be possibly made private if it's sufficient to expose only the methods listed above.

### Time zones!
Very important thing is displaying dates and time adequately to the time zone.

Calendar could be used in any place of the world and application should provide possibility to show dates and time for specific time zone.

That's why an application could take `Unspecified` kind of the date and time with pointed destination time zone as a param for Web API methods.
Calendar events are internally stored always in [UTC](http://en.wikipedia.org/wiki/UTC) and are always returned by the Web API in time zone specified by the caller.

### Performance (100-200 concurrent users)
RESTful ASP.NET Web API uses HTTP as its underlying communication.
Web API methods are very easy to be called asynchronously. 
The only bottleneck we should worry about is data storage - in our situation RavenDb.

100-200 concurrent users is not big deal because:
> RavenDB can be used for application that needs to store millions of records and has fast query times.
> RavenDB do this by using by pre-compute Map or MapReduce in the background. The indexes are running asynchronously. 

Source: http://ravendb.net/docs/article-page/2.5/csharp/intro/high-performance
