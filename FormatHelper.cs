using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MeetingRequest
{
    /// <summary>
    /// Helper to modify dataa to be validated as a RFC-2445 format.
    /// </summary>
    internal static class FormatHelper
    {
        /// <summary>
        /// Format for a DateTime
        /// </summary>
        public const string CAL_DATEFORMAT = "yyyyMMdd\\THHmmss\\Z";

        /// <summary>
        /// Format an double as RFC format
        /// </summary>
        public static string FormatDouble(double value)
        {
            return Convert.ToString(value, new CultureInfo("en-US"));
        }

        /// <summary>
        /// Format a timespan
        /// </summary>
        public static string FormatTimeSpan(TimeSpan timespan)
        {
            if (timespan == null)
                return null;

            //A duration of 15 days, 5 hours, and 20 seconds would be: P15DT5H0M20S

            //"P" (dur-date / dur-time / dur-week)

            string value = "P";

            if (timespan.Days > 0)
                value += timespan.Days + "D";

            //"T" (dur-hour / dur-minute / dur-second)
            if (timespan.Hours > 0 || timespan.Minutes > 0 || timespan.Seconds > 0)
            {
                value += "T";

                if (timespan.Hours > 0)
                    value += timespan.Hours + "H";

                if (timespan.Minutes > 0)
                    value += timespan.Minutes + "M";

                if (timespan.Seconds > 0)
                    value += timespan.Seconds + "S";
            }
            return value;
        }

        /// <summary>
        /// Helper to modify string to be validated as a RFC-2445 text format.
        /// </summary>
        public static string ReplaceForCal(this string originalString)
        {
            string value = originalString.Replace(@"\", @"\\");
            value = value.Replace(@"\\n", @"\n");
            value = value.Replace(@"\\N", @"\n");
            value = value.Replace(";", @"\;");
            value = value.Replace(",", @"\,");
            return value;
        }
    }
}
