using System;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   [DataContract]
   public class Todo : ICalendarElement
   {
      [DataMember]
      public string UID { get; set; }

      [DataMember]
      public DateTimeOffset? DtStamp { get; set; }

      [DataMember]
      public DateTimeOffset? StartTime { get; set; }

      [DataMember]
      public DateTimeOffset? DueTime { get; set; }

      [DataMember]
      public TimeSpan? Duration { get; set; }

      [DataMember]
      public string Summary { get; set; }

      [DataMember]
      public string Description { get; set; }

      string ICalendarElement.GetFormattedElement()
      {
         if (string.IsNullOrWhiteSpace(this.UID))
            throw new InvalidOperationException("VTODO requires UID.");

         if (this.DueTime.HasValue && this.Duration.HasValue)
            throw new InvalidOperationException("VTODO cannot contain both DUE and DURATION.");

         if (this.Duration.HasValue && !this.StartTime.HasValue)
            throw new InvalidOperationException("VTODO with DURATION requires DTSTART.");

         StringBuilder sb = new StringBuilder();
         sb.AppendLine("BEGIN:VTODO");
         sb.AppendLine("UID:" + this.UID);
         sb.AppendLine("DTSTAMP:" + (this.DtStamp ?? DateTimeOffset.UtcNow).ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.StartTime.HasValue)
            sb.AppendLine("DTSTART:" + this.StartTime.Value.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.DueTime.HasValue)
            sb.AppendLine("DUE:" + this.DueTime.Value.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.Duration.HasValue)
            sb.AppendLine("DURATION:" + FormatHelper.FormatTimeSpan(this.Duration.Value));

         if (!string.IsNullOrEmpty(this.Summary))
            sb.AppendLine("SUMMARY:" + this.Summary.ReplaceForCal());

         if (!string.IsNullOrEmpty(this.Description))
            sb.AppendLine("DESCRIPTION:" + this.Description.ReplaceForCal());

         sb.AppendLine("END:VTODO");
         return sb.ToString();
      }
   }
}
