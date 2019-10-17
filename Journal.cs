using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingRequest
{
   [DataContract]
   internal class Journal : ICalendarElement
   {
      string ICalendarElement.GetFormattedElement()
      {
         return null;
      }
   }
}
