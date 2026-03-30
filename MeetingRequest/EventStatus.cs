using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// This helper defines the event's status.
   /// </summary>
   internal class EventStatusHelper
   {
      /// <summary>
      /// Convert Enum to a string
      /// </summary>
      public static string Convert(EventStatus eventStatus)
      {
         switch (eventStatus)
         {
            case EventStatus.TENTATIVE:
               return "TENTATIVE";

            case EventStatus.CANCELLED:
               return "CANCELLED";

            case EventStatus.CONFIRMED:
               return "CONFIRMED";

            default:
               return null;
         }
      }
   }

   /// <summary>
   /// This property defines the overall status or confirmation for the calendar component.
   /// </summary>
   [DataContract]
   public enum EventStatus : int
   {
      /// <summary>
      /// Undefined
      /// </summary>
      [EnumMember]
      UNDEFINED = 0,
      /// <summary>
      /// Tentative
      /// </summary>
      [EnumMember]
      TENTATIVE = 1,
      /// <summary>
      /// Confirmed
      /// </summary>
      [EnumMember]
      CONFIRMED = 2,
      /// <summary>
      /// Cancelled
      /// </summary>
      [EnumMember]
      CANCELLED = 3
   }
}