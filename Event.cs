using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   /// <summary>
   /// Provide a grouping of component properties that describe an event.
   /// </summary>
   [DataContract]
   public class Event : ICalendarElement
   {
      /// <summary>
      /// Organizer.
      /// </summary>
      [DataMember]
      public Organizer Organizer { get; set; }

      /// <summary>
      /// Attendees.
      /// </summary>
      [DataMember]
      public List<Attendee> Attendees { get { return this.attendees; } }

      /// <summary>
      /// Categories.
      /// </summary>
      [DataMember]
      public List<string> Categories { get { return this.categories; } }

      /// <summary>
      /// Categories.
      /// </summary>
      [DataMember]
      public List<Alarm> Alarms { get { return this.alarms; } }

      /// <summary>
      /// Coordinate GPS
      /// </summary>
      [DataMember]
      public GeoCoordinate GpsCoordinate { get; set; }

      /// <summary>
      /// Location
      /// </summary>
      [DataMember]
      public string Location { get; set; }

      /// <summary>
      /// Unique ID
      /// </summary>
      [DataMember]
      public string UID { get; set; }

      /// <summary>
      /// Sequence (after any update of the event, SequenceNbr is increment)
      /// </summary>
      [DataMember]
      public int SequenceNbr { get; set; }

      /// <summary>
      /// Description
      /// </summary>
      [DataMember]
      public string Description { get; set; }

      /// <summary>
      /// Title
      /// </summary>
      [DataMember]
      public string Title { get; set; }

      /// <summary>
      /// Priority (High, Medium, Low)
      /// </summary>
      [DataMember]
      public PriorityLevel PriorityLevel { get; set; }

      /// <summary>
      /// Start time
      /// </summary>
      [DataMember]
      public DateTime StartTime { get; set; }

      /// <summary>
      /// End time
      /// </summary>
      [DataMember]
      public DateTime? EndTime { get; set; }

      /// <summary>
      /// Status (Confirmed, Canceled, ...
      /// </summary>
      [DataMember]
      public EventStatus Status { get; set; }

      /// <summary>
      /// Create a new event
      /// </summary>
      public Event()
      {
         this.PriorityLevel = PriorityLevel.Normal;
         this.Status = EventStatus.UNDEFINED;
      }

      private List<Attendee> attendees = new List<Attendee>();
      private List<string> categories = new List<string>();
      private List<Alarm> alarms = new List<Alarm>();

      /// <summary>
      /// Return an event class in RFC-2445 format
      /// </summary>
      string ICalendarElement.GetFormattedElement()
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("BEGIN:VEVENT");

         sb.AppendLine("SEQUENCE:" + this.SequenceNbr.ToString());
         sb.AppendLine("UID:" + this.UID);
         sb.AppendLine("DTSTAMP:" + this.StartTime.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if ( this.Organizer != null )
            sb.AppendLine("ORGANIZER;" + this.Organizer.GetFormattedElement());

         if ( this.attendees.Count > 0 )
            foreach ( var attendee in this.attendees )
               sb.AppendLine("ATTENDEE;" + attendee.GetFormattedElement());

         var eventStatus = EventStatusHelper.Convert(this.Status);
         if ( eventStatus != null )
            sb.AppendLine("STATUS:" + eventStatus);

         sb.AppendLine("DTSTART:" + this.StartTime.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if ( this.EndTime.HasValue )
            sb.AppendLine("DTEND:" + this.EndTime.Value.ToUniversalTime().ToString(FormatHelper.CAL_DATEFORMAT));

         if ( !string.IsNullOrEmpty(this.Location) )
            sb.AppendLine("LOCATION:" + this.Location.ReplaceForCal());

         if ( this.GpsCoordinate != null )
            sb.AppendLine("GEO:" + this.GpsCoordinate.GetFormattedElement());

         if ( this.categories.Count > 0 )
            sb.Append("CATEGORIES:" + string.Join(",", this.categories.Select(elt => elt.ToUpperInvariant().ReplaceForCal()).ToArray()));

         if ( !string.IsNullOrEmpty(this.Description) )
            sb.AppendLine("DESCRIPTION:" + this.Description.ReplaceForCal());

         if ( !string.IsNullOrEmpty(this.Title) )
            sb.AppendLine("SUMMARY:" + this.Title.ReplaceForCal());


         sb.AppendLine("PRIORITY:" + ( (int)this.PriorityLevel ));
         sb.AppendLine("CLASS:PUBLIC");
         sb.AppendLine("TRANSP:OPAQUE");

         foreach ( var alarm in this.alarms )
            sb.Append(alarm.GetFormattedElement());

         sb.AppendLine("END:VEVENT");

         return sb.ToString();
      }
   }
}
