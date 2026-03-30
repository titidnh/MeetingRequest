using System;
using MeetingRequest;
using NUnit.Framework;

namespace MeetingRequest.Tests
{
    public class CalendarRfcComplianceTests
    {
        [Test]
        public void Calendar_Throws_When_ProdId_Missing()
        {
            var calendar = new Calendar { Version = "2.0" };

            Assert.Throws<InvalidOperationException>(() => calendar.GetCalendarContentText());
        }

        [Test]
        public void Calendar_Throws_When_Version_Missing()
        {
            var calendar = new Calendar { ProdID = "MyCompany//MyApp//EN" };

            Assert.Throws<InvalidOperationException>(() => calendar.GetCalendarContentText());
        }

        [Test]
        public void Calendar_Uses_Crlf_And_Contains_VCalendar_Block()
        {
            var calendar = CreateCalendarWithSimpleEvent();

            var ics = calendar.GetCalendarContentText();

            Assert.IsTrue(ics.Contains("BEGIN:VCALENDAR\r\n"));
            Assert.IsTrue(ics.Contains("END:VCALENDAR"));
            Assert.IsFalse(ics.Contains("\r\r\n"));
        }

        [Test]
        public void Calendar_Folds_Long_Lines()
        {
            var calendar = new Calendar
            {
                ProdID = "MyCompany//MyApp//EN",
                Version = "2.0",
                CalendarMethod = CalendarMethod.REQUEST
            };

            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                Title = new string('A', 120)
            };

            calendar.Elements.Add(ev);

            var ics = calendar.GetCalendarContentText();

            Assert.IsTrue(ics.Contains("SUMMARY:"));
            Assert.IsTrue(ics.Contains("\r\n "));
        }

        [Test]
        public void Event_Throws_When_Uid_Missing()
        {
            var ev = new Event
            {
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero)
            };

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)ev).GetFormattedElement());
        }

        [Test]
        public void Event_Throws_When_DtStart_Default()
        {
            var ev = new Event
            {
                UID = "uid-1"
            };

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)ev).GetFormattedElement());
        }

        [Test]
        public void Event_Uses_DtStamp_When_Provided()
        {
            var dtStamp = new DateTimeOffset(2025, 12, 24, 12, 30, 0, TimeSpan.Zero);
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                DtStamp = dtStamp
            };

            var ics = ((ICalendarElement)ev).GetFormattedElement();

            Assert.IsTrue(ics.Contains("DTSTAMP:20251224T123000Z"));
        }

        [Test]
        public void Event_Writes_Categories_With_LineBreak_Before_Description()
        {
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                Description = "Desc",
                Title = "Title"
            };
            ev.Categories.Add("dev");

            var ics = ((ICalendarElement)ev).GetFormattedElement();

            Assert.IsTrue(ics.Contains("CATEGORIES:DEV"));
            Assert.IsTrue(ics.Contains("CATEGORIES:DEV" + Environment.NewLine + "DESCRIPTION:Desc"));
        }

        [Test]
        public void Event_Escapes_Text_Fields()
        {
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                Description = "line1\nline2;comma,slash\\",
                Title = "sum;mary,1"
            };

            var ics = ((ICalendarElement)ev).GetFormattedElement();

            Assert.IsTrue(ics.Contains("DESCRIPTION:line1\\nline2\\;comma\\,slash\\\\"));
            Assert.IsTrue(ics.Contains("SUMMARY:sum\\;mary\\,1"));
        }

        [Test]
        public void DisplayAlarm_Throws_When_Description_Missing()
        {
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero)
            };
            ev.Alarms.Add(new BeforePeriodAlarm { BeforeTime = TimeSpan.FromMinutes(15) });

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)ev).GetFormattedElement());
        }

        [Test]
        public void Alarm_Throws_When_Repeat_Without_Duration()
        {
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero)
            };
            ev.Alarms.Add(new BeforePeriodAlarm
            {
                BeforeTime = TimeSpan.FromMinutes(15),
                Repeat = 2,
                Description = "Reminder"
            });

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)ev).GetFormattedElement());
        }

        [Test]
        public void Alarm_Throws_When_Duration_Without_Repeat()
        {
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero)
            };
            ev.Alarms.Add(new BeforePeriodAlarm
            {
                BeforeTime = TimeSpan.FromMinutes(15),
                Duration = TimeSpan.FromMinutes(5),
                Description = "Reminder"
            });

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)ev).GetFormattedElement());
        }

        [Test]
        public void Alarm_Writes_Trigger_Repeat_Duration_And_Description()
        {
            var ev = new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero)
            };
            ev.Alarms.Add(new BeforePeriodAlarm
            {
                BeforeTime = TimeSpan.FromMinutes(15),
                Repeat = 2,
                Duration = TimeSpan.FromMinutes(5),
                Description = "Reminder"
            });

            var ics = ((ICalendarElement)ev).GetFormattedElement();

            Assert.IsTrue(ics.Contains("BEGIN:VALARM"));
            Assert.IsTrue(ics.Contains("TRIGGER:-PT15M"));
            Assert.IsTrue(ics.Contains("REPEAT:2"));
            Assert.IsTrue(ics.Contains("DURATION:PT5M"));
            Assert.IsTrue(ics.Contains("DESCRIPTION:Reminder"));
            Assert.IsTrue(ics.Contains("END:VALARM"));
        }

        [Test]
        public void Todo_Throws_When_Uid_Missing()
        {
            var todo = new Todo();

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)todo).GetFormattedElement());
        }

        [Test]
        public void Todo_Throws_When_Due_And_Duration_Both_Set()
        {
            var todo = new Todo
            {
                UID = "todo-1",
                DueTime = new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero),
                Duration = TimeSpan.FromHours(1)
            };

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)todo).GetFormattedElement());
        }

        [Test]
        public void Todo_Throws_When_Duration_Without_DtStart()
        {
            var todo = new Todo
            {
                UID = "todo-1",
                Duration = TimeSpan.FromHours(1)
            };

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)todo).GetFormattedElement());
        }

        [Test]
        public void Todo_Writes_Valid_Content()
        {
            var todo = new Todo
            {
                UID = "todo-1",
                DtStamp = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero),
                StartTime = new DateTimeOffset(2026, 1, 2, 8, 0, 0, TimeSpan.Zero),
                Duration = TimeSpan.FromHours(2),
                Summary = "Task",
                Description = "Do work"
            };

            var ics = ((ICalendarElement)todo).GetFormattedElement();

            Assert.IsTrue(ics.Contains("BEGIN:VTODO"));
            Assert.IsTrue(ics.Contains("UID:todo-1"));
            Assert.IsTrue(ics.Contains("DTSTAMP:20260101T080000Z"));
            Assert.IsTrue(ics.Contains("DTSTART:20260102T080000Z"));
            Assert.IsTrue(ics.Contains("DURATION:PT2H"));
            Assert.IsTrue(ics.Contains("SUMMARY:Task"));
            Assert.IsTrue(ics.Contains("DESCRIPTION:Do work"));
            Assert.IsTrue(ics.Contains("END:VTODO"));
        }

        [Test]
        public void Journal_Throws_When_Uid_Missing()
        {
            var journal = new Journal();

            Assert.Throws<InvalidOperationException>(() => ((ICalendarElement)journal).GetFormattedElement());
        }

        [Test]
        public void Journal_Writes_Valid_Content()
        {
            var journal = new Journal
            {
                UID = "journal-1",
                DtStamp = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero),
                StartTime = new DateTimeOffset(2026, 1, 1, 9, 0, 0, TimeSpan.Zero),
                Summary = "Note",
                Description = "Entry"
            };

            var ics = ((ICalendarElement)journal).GetFormattedElement();

            Assert.IsTrue(ics.Contains("BEGIN:VJOURNAL"));
            Assert.IsTrue(ics.Contains("UID:journal-1"));
            Assert.IsTrue(ics.Contains("DTSTAMP:20260101T080000Z"));
            Assert.IsTrue(ics.Contains("DTSTART:20260101T090000Z"));
            Assert.IsTrue(ics.Contains("SUMMARY:Note"));
            Assert.IsTrue(ics.Contains("DESCRIPTION:Entry"));
            Assert.IsTrue(ics.Contains("END:VJOURNAL"));
        }

        [Test]
        public void FreeBusy_Throws_When_Uid_Missing()
        {
            var fb = new FreeBusy();

            Assert.Throws<InvalidOperationException>(() => fb.GetFormattedElement());
        }

        [Test]
        public void FreeBusy_Throws_When_DtEnd_Before_DtStart()
        {
            var fb = new FreeBusy
            {
                UID = "fb-1",
                StartTime = new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero),
                EndTime = new DateTimeOffset(2026, 1, 2, 9, 0, 0, TimeSpan.Zero)
            };

            Assert.Throws<InvalidOperationException>(() => fb.GetFormattedElement());
        }

        [Test]
        public void FreeBusy_Writes_Valid_Content_With_Periods()
        {
            var fb = new FreeBusy
            {
                UID = "fb-1",
                DtStamp = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero),
                StartTime = new DateTimeOffset(2026, 1, 2, 8, 0, 0, TimeSpan.Zero),
                EndTime = new DateTimeOffset(2026, 1, 2, 18, 0, 0, TimeSpan.Zero)
            };

            fb.Attendees.Add(new Attendee { Email = "attendee@example.com", PublicName = "John" });
            fb.Periods.Add(new FreeBusyPeriod(
                new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2026, 1, 2, 11, 0, 0, TimeSpan.Zero)));

            var ics = fb.GetFormattedElement();

            Assert.IsTrue(ics.Contains("BEGIN:VFREEBUSY"));
            Assert.IsTrue(ics.Contains("UID:fb-1"));
            Assert.IsTrue(ics.Contains("DTSTAMP:20260101T080000Z"));
            Assert.IsTrue(ics.Contains("DTSTART:20260102T080000Z"));
            Assert.IsTrue(ics.Contains("DTEND:20260102T180000Z"));
            Assert.IsTrue(ics.Contains("ATTENDEE;CN=John:MAILTO:attendee@example.com"));
            Assert.IsTrue(ics.Contains("FREEBUSY:20260102T100000Z/20260102T110000Z"));
            Assert.IsTrue(ics.Contains("END:VFREEBUSY"));
        }

        [Test]
        public void FreeBusyPeriod_Throws_When_End_Before_Start()
        {
            Assert.Throws<ArgumentException>(() => new FreeBusyPeriod(
                new DateTimeOffset(2026, 1, 2, 11, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)));
        }

        [Test]
        public void Event_Writes_Geo_Using_Semicolon()
        {
            var ev = new Event
            {
                UID = "uid-geo",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                GpsCoordinate = new GeoCoordinate(48.8566, 2.3522)
            };

            var ics = ((ICalendarElement)ev).GetFormattedElement();

            Assert.IsTrue(ics.Contains("GEO:48.8566;2.3522"));
        }

        [Test]
        public void Attendee_Formats_Correctly()
        {
            var attendee = new Attendee { Email = "a@b.com", PublicName = "Alice", Role = Role.REQPARTICIPANT, NeedAnswer = true };
            var formatted = attendee.GetFormattedElement();

            Assert.IsTrue(formatted.Contains("ROLE=REQ-PARTICIPANT"));
            Assert.IsTrue(formatted.Contains("CN=Alice"));
            Assert.IsTrue(formatted.Contains("RSVP=TRUE"));
            Assert.IsTrue(formatted.Contains("MAILTO:a@b.com"));
        }

        private static Calendar CreateCalendarWithSimpleEvent()
        {
            var calendar = new Calendar
            {
                ProdID = "MyCompany//MyApp//EN",
                Version = "2.0",
                CalendarMethod = CalendarMethod.REQUEST
            };

            calendar.Elements.Add(new Event
            {
                UID = "uid-1",
                StartTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                Title = "Title"
            });

            return calendar;
        }
    }
}
