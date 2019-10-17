using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// Priority
   /// </summary>
   [DataContract]
   public enum PriorityLevel : int
   {
      /// <summary>
      /// Normal
      /// </summary>
      [EnumMember]
      Normal = 5,
      /// <summary>
      /// High
      /// </summary>
      [EnumMember]
      High = 1,
      /// <summary>
      /// Low
      /// </summary>
      [EnumMember]
      Low = 9
   }
}