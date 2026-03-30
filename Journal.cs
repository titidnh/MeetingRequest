using System;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   [DataContract]
   public class Journal : ICalendarElement
   {
      [DataMember]
      public string UID { get; set; }

      [DataMember]
      public DateTimeOffset? DtStamp { get; set; }

      [DataMember]
      public DateTimeOffset? StartTime { get; set; }

      [DataMember]
      public string Summary { get; set; }

      [DataMember]
      public string Description { get; set; }

      string ICalendarElement.GetFormattedElement()
      {
         if (string.IsNullOrWhiteSpace(this.UID))
            throw new InvalidOperationException("VJOURNAL requires UID.");

         StringBuilder sb = new StringBuilder();
         sb.AppendLine("BEGIN:VJOURNAL");
         sb.AppendLine("UID:" + this.UID);
         sb.AppendLine("DTSTAMP:" + (this.DtStamp ?? DateTimeOffset.UtcNow).ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (this.StartTime.HasValue)
            sb.AppendLine("DTSTART:" + this.StartTime.Value.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if (!string.IsNullOrEmpty(this.Summary))
            sb.AppendLine("SUMMARY:" + this.Summary.ReplaceForCal());

         if (!string.IsNullOrEmpty(this.Description))
            sb.AppendLine("DESCRIPTION:" + this.Description.ReplaceForCal());

         sb.AppendLine("END:VJOURNAL");
         return sb.ToString();
      }
   }
}
