using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MeetingRequest
{
   /// <summary>
   /// Organizer for a calendar component.
   /// </summary>
   [DataContract]
   public class Organizer
   {
      /// <summary>
      /// Public name (CN)
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
      /// Create a new organizer
      /// </summary>
      public Organizer()
      {
         this.Role = Role.CHAIR;
      }

      /// <summary>
      /// Return an organizer class in RFC-2445 format
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
