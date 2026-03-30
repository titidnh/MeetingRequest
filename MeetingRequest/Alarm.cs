using System;
using System.Runtime.Serialization;
using System.Text;

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

         bool hasRepeat = this.Repeat.HasValue;
         bool hasDuration = this.Duration != TimeSpan.Zero;

         if (hasRepeat != hasDuration)
            throw new InvalidOperationException("VALARM requires both DURATION and REPEAT when one is present.");

         if (hasRepeat)
         {
            sb.AppendLine("REPEAT:" + this.Repeat.Value);
            sb.AppendLine("DURATION:" + FormatHelper.FormatTimeSpan(this.Duration));
         }

         sb.AppendLine("ACTION:DISPLAY");

         if (string.IsNullOrEmpty(this.Description))
            throw new InvalidOperationException("DISPLAY alarm requires DESCRIPTION.");

         sb.AppendLine("DESCRIPTION:" + this.Description.ReplaceForCal());

         sb.AppendLine("END:VALARM");
         return sb.ToString();
      }
   }
}
