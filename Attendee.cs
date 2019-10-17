using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   /// <summary>
   /// Property Name: ATTENDEE
   /// Purpose: The property defines an "Attendee" within a calendar component.
   /// Value Type: CAL-ADDRESS
   /// </summary>
   [DataContract]
   public class Attendee
   {
      /// <summary>
      /// CN Name
      /// </summary>
      [DataMember]
      public string PublicName { get; set; }

      /// <summary>
      /// Email
      /// </summary>
      [DataMember]
      public string Email { get; set; }

      /// <summary>
      /// Role
      /// </summary>
      [DataMember]
      public Role Role { get; set; }

      /// <summary>
      /// Need an answer or not for an event
      /// </summary>
      [DataMember]
      public bool? NeedAnswer { get; set; }

      /// <summary>
      /// Create a new Attendee
      /// </summary>
      public Attendee()
      {
         this.Role = Role.UNDEFINED;
      }

      /// <summary>
      /// Return an attendee class in RFC-2445 format
      /// </summary>
      internal string GetFormattedElement()
      {
         StringBuilder sb = new StringBuilder();

         List<string> items = new List<string>();
         var role = RoleHelper.Convert(this.Role);
         if (role != null)
            items.Add("ROLE=" + role);

         if (!string.IsNullOrEmpty(this.PublicName))
            items.Add("CN=" + this.PublicName.ReplaceForCal());

         if (this.NeedAnswer.HasValue)
            items.Add("RSVP=" + (this.NeedAnswer.GetValueOrDefault() ? "TRUE" : "FALSE"));

         for (int i = 0; i < items.Count; ++i)
         {
            sb.Append(items[i]);
            sb.Append((i == items.Count - 1) ? ":" : ";");
         }

         sb.Append("MAILTO:" + this.Email.ReplaceForCal());
         return sb.ToString();
      }
   }
}