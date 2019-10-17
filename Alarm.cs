using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// Alarm for an event
   /// </summary>
   [DataContract]
   [KnownType(typeof(BeforePeriodAlarm))]
   [KnownType(typeof(SpecificDateTimeAlarm))]
   public abstract class Alarm
   {
      /// <summary>
      /// Number of repeat
      /// </summary>
      [DataMember]
      public int? Repeat { get; set; }

      /// <summary>
      /// Interval between Repeat
      /// </summary>
      [DataMember]
      public TimeSpan Duration { get; set; }

      /// <summary>
      /// Description
      /// </summary>
      [DataMember]
      public string Description { get; set; }

      /// <summary>
      /// Format the trigger data
      /// </summary>
      protected abstract string GetTriggerString();

      /// <summary>
      /// Return an alarm class in RFC-2445 format
      /// </summary>
      internal string GetFormattedElement()
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("BEGIN:VALARM");
         string trigger = this.GetTriggerString();
         if ( !string.IsNullOrEmpty(trigger) )
            sb.AppendLine("TRIGGER" + trigger);
         if ( this.Repeat.HasValue )
            sb.AppendLine("REPEAT:" + this.Repeat.Value);

         var duration = FormatHelper.FormatTimeSpan(this.Duration);
         if ( !string.IsNullOrEmpty(duration) )
            sb.AppendLine("DURATION:" + duration);

         sb.AppendLine("ACTION:DISPLAY");
         if ( !string.IsNullOrEmpty(this.Description) )
            sb.AppendLine("" + this.Description);

         sb.AppendLine("END:VALARM");
         return sb.ToString();
      }
   }
}
