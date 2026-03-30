using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// Helper for the role
   /// </summary>
   internal class RoleHelper
   {
      public static string Convert(Role role)
      {
         switch (role)
         {
            case Role.CHAIR:
               return "CHAIR";

            case Role.NONPARTICIPANT:
               return "NON-PARTICIPANT";

            case Role.OPTPARTICIPANT:
               return "OPT-PARTICIPANT";

            case Role.REQPARTICIPANT:
               return "REQ-PARTICIPANT";

            default:
               return null;
         }
      }
   }

   /// <summary>
   /// Role
   /// </summary>
   [DataContract]
   public enum Role : int
   {
      /// <summary>
      /// Undefined
      /// </summary>
      [EnumMember]
      UNDEFINED = 0,
      /// <summary>
      /// Chair
      /// </summary>
      [EnumMember]
      CHAIR = 1,
      /// <summary>
      /// Required participant
      /// </summary>
      [EnumMember]
      REQPARTICIPANT = 2,
      /// <summary>
      /// Optional participant
      /// </summary>
      [EnumMember]
      OPTPARTICIPANT = 3,
      /// <summary>
      /// Non participant
      /// </summary>
      [EnumMember]
      NONPARTICIPANT = 4
   }
}
