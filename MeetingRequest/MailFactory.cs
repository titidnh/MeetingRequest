using MeetingRequest;

namespace System.Net.Mail
{
   /// <summary>
   /// Entry point for MeetingRequest module
   /// </summary>
   public static class MailFactory
   {
      /// <summary>
      /// Extension method to add an embedded calendar to a MailMessage
      /// </summary>
      /// <param name="message">Mail message</param>
      /// <param name="calendar">Calendar</param>
      public static void AddCalendar(this MailMessage message, Calendar calendar)
      {
         AlternateView alternative = new AlternateView(calendar.GetCalendarContentStream(), new System.Net.Mime.ContentType("text/calendar; method=REQUEST;charset=\"utf-8\""));
         message.AlternateViews.Add(alternative);
      }

      /// <summary>
      /// Extension method to add a calendar as attachment to a MailMessage
      /// </summary>
      /// <param name="message">Mail message</param>
      /// <param name="calendar">Calendar</param>
      public static void AddCalendarAsAttachment(this MailMessage message, Calendar calendar)
      {
         message.Attachments.Add(new Attachment(calendar.GetCalendarContentStream(), "meeting.ics", "text/calendar"));
      }

      /// <summary>
      /// Extension method to add a embedded event to a MailMessage
      /// </summary>
      /// <param name="message">Mail message</param>
      /// <param name="calEvent">Event</param>
      public static void AddEvent(this MailMessage message, Event calEvent)
      {
         message.AddCalendar(CreateCalendar(calEvent));
      }

      /// <summary>
      /// Extension method to add an event as attachment to a MailMessage
      /// </summary>
      /// <param name="message">Mail message</param>
      /// <param name="calEvent">Event</param>
      public static void AddEventAsAttachment(this MailMessage message, Event calEvent)
      {
         message.AddCalendarAsAttachment(CreateCalendar(calEvent));
      }

      private static Calendar CreateCalendar(Event calEvent)
      {
         Calendar calendar = new Calendar()
         {
            Version = "2.0",
            ProdID = "MeetingRequest CodePlex//MailRequest 1.0 MIMEDIR//EN"
         };

         if ( calEvent.Status == EventStatus.CANCELLED )
            calendar.CalendarMethod = CalendarMethod.CANCEL;

         calendar.Elements.Add(calEvent);

         return calendar;
      }
   }
}
