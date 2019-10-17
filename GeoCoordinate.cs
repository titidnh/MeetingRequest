using System.Runtime.Serialization;

namespace MeetingRequest
{
   /// <summary>
   /// Coordinate of an event.
   /// </summary>
   [DataContract]
   public class GeoCoordinate
   {
      /// <summary>
      /// Create a new GPS position
      /// </summary>
      /// <param name="latitude">Latitude</param>
      /// <param name="longitude">Longitude</param>
      public GeoCoordinate(double latitude, double longitude)
      {
         this.Lat = latitude;
         this.Long = longitude;
      }

      /// <summary>
      /// Longitude
      /// </summary>
      [DataMember]
      public double Long { get; set; }

      /// <summary>
      /// Latitude
      /// </summary>
      [DataMember]
      public double Lat { get; set; }

      /// <summary>
      /// Return an alarm class in RFC-2445 format
      /// </summary>
      internal string GetFormattedElement()
      {
         //;Latitude and Longitude components
         return string.Format("{0};{1}", FormatHelper.FormatDouble(this.Lat), FormatHelper.FormatDouble(this.Long));
      }
   }
}
