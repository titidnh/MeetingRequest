# MeetingRequest

A small library for creating and formatting iCalendar (RFC 5545 / RFC 2445) calendar objects (events, alarms, attendees, organizers, etc.).

## Features
- Build iCalendar objects (VCALENDAR, VEVENT, VALARM...)
- Escape and format values for iCalendar compatibility
- Support for timezone-aware date/time using `DateTimeOffset`

## Supported targets
This repository contains projects targeting multiple frameworks. The library is compatible with:
- .NET Standard 2.0
- .NET Standard 2.1
- .NET 6
- .NET 8
- .NET 10

## Building
Prerequisites: installed .NET SDK (recommended: .NET 8).

To restore and build:

```bash
dotnet restore
dotnet build -c Release
```

To create NuGet packages:

```bash
dotnet pack -c Release -o ./artifacts
```

## Usage
Use the library to construct calendar objects and get the iCalendar text or stream:

```csharp
var calendar = new MeetingRequest.Calendar
{
    ProdID = "MyCompany//MyApp//EN",
    Version = "2.0",
    CalendarMethod = MeetingRequest.CalendarMethod.REQUEST
};

// add events/attendees/alarms...
string icalText = calendar.GetCalendarContentText();
using var stream = calendar.GetCalendarContentStream();
```

## Notes & Recommendations
- Date/time fields use `DateTimeOffset` to preserve timezone/offset information.
- Strings are escaped for iCalendar compliance by the library, but callers should validate required fields (e.g. `UID`, `Email`).
- Consider adding unit tests and enabling nullable reference types for improved safety.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.
