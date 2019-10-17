using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// Alarm before an event (example : "Before 15 min")
   /// </summary>
   [DataContract]
   public class BeforePeriodAlarm : Alarm
   {
      /// <summary>
      /// Before a Date
      /// </summary>
      [DataMember]
      public TimeSpan BeforeTime { get; set; }

      /// <summary>
      /// Format the trigger data
      /// </summary>
      protected override string GetTriggerString()
      {
         return ":-" + FormatHelper.FormatTimeSpan(this.BeforeTime);
      }
   }
}
