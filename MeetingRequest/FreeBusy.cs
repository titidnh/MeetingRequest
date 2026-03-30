using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   [DataContract]
   public class FreeBusy : ICalendarElement
   {
      private readonly List<Attendee> attendees = new List<Attendee>();
      private readonly List<FreeBusyPeriod> periods = new List<FreeBusyPeriod>();

      [DataMember]
      public string UID { get; set; }

      [DataMember]
      public DateTimeOffset? DtStamp { get; set; }

      [DataMember]
      public DateTimeOffset? StartTime { get; set; }

      [DataMember]
      public DateTimeOffset? EndTime { get; set; }

      [DataMember]
      public Organizer Organizer { get; set; }

      [DataMember]
      public IList<Attendee> Attendees { get { return this.attendees; } }

      [DataMember]
      public IList<FreeBusyPeriod> Periods { get { return this.periods; } }

      public string GetFormattedElement()
      {
         if (string.IsNullOrWhiteSpace(this.UID))
            throw new InvalidOperationException("VFREEBUSY requires UID.");

         if (this.StartTime.HasValue && this.EndTime.HasValue && this.EndTime.Value < this.StartTime.Value)
            throw new InvalidOperationException("VFREEBUSY DTEND must be greater than or equal to DTSTART.");

         StringBuilder sb = new StringBuilder();
         sb.AppendLine("BEGIN:VFREEBUSY");
         sb.AppendLine("UID:" + this.UID);
         sb.AppendLine("DTSTAMP:" + (this.DtStamp ?? DateTimeOffset.UtcNow).ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.StartTime.HasValue)
            sb.AppendLine("DTSTART:" + this.StartTime.Value.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.EndTime.HasValue)
            sb.AppendLine("DTEND:" + this.EndTime.Value.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.Organizer != null)
            sb.AppendLine("ORGANIZER;" + this.Organizer.GetFormattedElement());

         foreach (var attendee in this.attendees)
            sb.AppendLine("ATTENDEE;" + attendee.GetFormattedElement());

         if (this.periods.Count > 0)
         {
            List<string> items = new List<string>(this.periods.Count);
            for (int i = 0; i < this.periods.Count; i++)
               items.Add(this.periods[i].GetFormattedElement());

            sb.AppendLine("FREEBUSY:" + string.Join(",", items));
         }

         sb.AppendLine("END:VFREEBUSY");
         return sb.ToString();
      }
   }

   [DataContract]
   public class FreeBusyPeriod
   {
      public FreeBusyPeriod(DateTimeOffset start, DateTimeOffset end)
      {
         if (end < start)
            throw new ArgumentException("Period end must be greater than or equal to start.");

         this.Start = start;
         this.End = end;
      }

      [DataMember]
      public DateTimeOffset Start { get; set; }

      [DataMember]
      public DateTimeOffset End { get; set; }

      internal string GetFormattedElement()
      {
         return this.Start.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT) + "/" + this.End.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT);
      }
   }
}
