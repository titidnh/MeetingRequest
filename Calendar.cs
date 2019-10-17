using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   /// <summary>
   /// The Calendaring Core Object is a collection of scheduling information.
   /// </summary>
   [DataContract]
   public class Calendar
   {
      private List<ICalendarElement> feeds = new List<ICalendarElement>();

      /// <summary>
      /// This property specifies the identifier for the product that created the iCalendar object.
      /// </summary>
      /// <example>      
      /// "Microsoft Corporation//Outlook9.0 MIMEDIR//EN"      
      /// </example>
      [DataMember]
      public string ProdID { get; set; }

      /// <summary>
      /// This property specifies the identifier corresponding to the highest version number or the minimum and maximum range 
      /// of the iCalendar specification that is required in order to interpret the iCalendar object.
      /// </summary>
      [DataMember]
      public string Version { get; set; }

      /// <summary>
      /// This property defines the iCalendar object method associated with the calendar object.
      /// </summary>
      [DataMember]
      public CalendarMethod CalendarMethod { get; set; }

      /// <summary>
      /// Collection of Events, Todo, Journal, ...
      /// </summary>
      [DataMember]
      public IList<ICalendarElement> Elements { get { return this.feeds; } }

      /// <summary>
      /// Return the calendar as a Stream
      /// </summary>
      public Stream GetCalendarContentStream()
      {
         MemoryStream ms = new MemoryStream();
         StreamWriter writer = new StreamWriter(ms);
         writer.Write(this.GetCalendarContentText());
         writer.Flush();
         ms.Seek(0, SeekOrigin.Begin);
         return ms;
      }

      /// <summary>
      /// Return the calendar as a string
      /// </summary>
      public string GetCalendarContentText()
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("BEGIN:VCALENDAR");
         if (!string.IsNullOrEmpty(this.ProdID))
            sb.AppendLine("PRODID:-//" + this.ProdID);
         if (!string.IsNullOrEmpty(this.Version))
            sb.AppendLine("VERSION:" + this.Version);
         sb.AppendLine("METHOD:" + CalendarMethodHelper.CalendarMethod(this.CalendarMethod));

         foreach (ICalendarElement element in this.Elements)
            sb.Append(element.GetFormattedElement());

         sb.Append("END:VCALENDAR");
         return sb.ToString();
      }
   }
}