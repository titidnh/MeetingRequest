using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// Alarm launched at a specific time
   /// </summary>
   [DataContract]
   public class SpecificDateTimeAlarm : Alarm
   {
      /// <summary>
      /// Specific DateTime for the alarm
      /// </summary>
      [DataMember]
      public DateTime SpecificDateTime { get; set; }

      /// <summary>
      /// Format the trigger data
      /// </summary>
      protected override string GetTriggerString()
      {
         return ";VALUE=DATE-TIME:" + this.SpecificDateTime.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT);
      }
   }
}
