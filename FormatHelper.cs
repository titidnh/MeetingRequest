using System;
using System.Globalization;
using System.Text;

namespace MeetingRequest
{
    /// <summary>
    /// Helper to modify dataa to be validated as a RFC-2445 format.
    /// </summary>
    internal static class FormatHelper
    {
        /// <summary>
        /// Format for a DateTime (UTC - RFC/ICAL format)
        /// </summary>
        public const string CAL_DATEFORMAT = "yyyyMMdd'T'HHmmss'Z'";

        public const string CalNewLine = "\r\n";

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
            var absoluteTimeSpan = timespan.Duration();

            string value = "P";

            if (absoluteTimeSpan.Days > 0)
                value += absoluteTimeSpan.Days + "D";

            if (absoluteTimeSpan.Hours > 0 || absoluteTimeSpan.Minutes > 0 || absoluteTimeSpan.Seconds > 0)
            {
                value += "T";

                if (absoluteTimeSpan.Hours > 0)
                    value += absoluteTimeSpan.Hours + "H";

                if (absoluteTimeSpan.Minutes > 0)
                    value += absoluteTimeSpan.Minutes + "M";

                if (absoluteTimeSpan.Seconds > 0)
                    value += absoluteTimeSpan.Seconds + "S";
            }

            if (value == "P")
                return "PT0S";

            return value;
        }

        /// <summary>
        /// Helper to modify string to be validated as a RFC-2445 text format.
        /// </summary>
        public static string ReplaceForCal(this string originalString)
        {
            string value = originalString.Replace("\\", "\\\\");
            value = value.Replace("\r\n", "\\n");
            value = value.Replace("\n", "\\n");
            value = value.Replace("\r", "\\n");
            value = value.Replace(";", "\\;");
            value = value.Replace(",", "\\,");
            return value;
        }

        public static string NormalizeAndFoldContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            string normalized = content.Replace("\r\n", "\n").Replace("\r", "\n");
            string[] lines = normalized.Split(new[] { '\n' }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.Length == 0)
                {
                    if (i < lines.Length - 1)
                        sb.Append(CalNewLine);
                    continue;
                }

                AppendFoldedLine(sb, line);

                if (i < lines.Length - 1)
                    sb.Append(CalNewLine);
            }

            return sb.ToString();
        }

        private static void AppendFoldedLine(StringBuilder sb, string line)
        {
            const int maxOctetsPerLine = 75;

            int index = 0;
            bool firstSegment = true;

            while (index < line.Length)
            {
                int currentLength = firstSegment ? 0 : 1;
                int segmentStart = index;

                while (index < line.Length)
                {
                    int charByteCount = Encoding.UTF8.GetByteCount(new[] { line[index] });
                    if (currentLength + charByteCount > maxOctetsPerLine)
                        break;

                    currentLength += charByteCount;
                    index++;
                }

                if (!firstSegment)
                    sb.Append(' ');

                if (index > segmentStart)
                {
                    sb.Append(line.Substring(segmentStart, index - segmentStart));
                }
                else
                {
                    sb.Append(line[index]);
                    index++;
                }

                if (index < line.Length)
                    sb.Append(CalNewLine);

                firstSegment = false;
            }
        }
    }
}
