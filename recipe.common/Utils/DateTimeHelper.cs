using System;
using System.Globalization;

namespace ch.thommenmedia.common.Utils
{
    /// <summary>
    ///     Contains Helper Util functions for DateTime and TimeSpan Datatypes
    /// </summary>
    public static class DateTimeHelper
    {
        public enum DateTimePart
        {
            YEAR,
            MONTH,
            DAY,
            HOUR,
            MINUTE,
            SECOND
        }

        /// <summary>
        ///     used to change any part of a datetime object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="part"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime SetDateTimePart(DateTime input, DateTimePart part, int value)
        {
            var y = input.Year;
            var m = input.Month;
            var d = input.Day;
            var h = input.Hour;
            var min = input.Minute;
            var s = input.Second;
            switch (part)
            {
                case DateTimePart.YEAR:
                    y = value;
                    break;
                case DateTimePart.MONTH:
                    m = value;
                    break;
                case DateTimePart.DAY:
                    d = value;
                    break;
                case DateTimePart.HOUR:
                    h = value;
                    break;
                case DateTimePart.MINUTE:
                    min = value;
                    break;
                case DateTimePart.SECOND:
                    s = value;
                    break;
            }
            return new DateTime(y, m, d, h, min, s);
        }

        /// <summary>
        ///     used to change any part of a datetime object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="part"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTimeOffset SetDateTimePart(DateTimeOffset input, DateTimePart part, int value)
        {
            var y = input.Year;
            var m = input.Month;
            var d = input.Day;
            var h = input.Hour;
            var min = input.Minute;
            var s = input.Second;
            switch (part)
            {
                case DateTimePart.YEAR:
                    y = value;
                    break;
                case DateTimePart.MONTH:
                    m = value;
                    break;
                case DateTimePart.DAY:
                    d = value;
                    break;
                case DateTimePart.HOUR:
                    h = value;
                    break;
                case DateTimePart.MINUTE:
                    min = value;
                    break;
                case DateTimePart.SECOND:
                    s = value;
                    break;
            }
            return new DateTimeOffset(y, m, d, h, min, s, input.Offset);
        }

        /// <summary>
        ///     Calculates if the given time of day is between a defined start and end time
        /// </summary>
        /// <param name="dateTime">Given time to check</param>
        /// <param name="start">Start slot of time</param>
        /// <param name="end">End slot of time</param>
        /// <returns>Return true if the given dateTime is between the start and end timeslot </returns>
        public static bool TimeBetween(DateTime dateTime, TimeSpan start, TimeSpan end)
        {
            // Source : http://stackoverflow.com/questions/12998739/how-to-check-if-datetime-now-is-between-two-given-datetimes-for-time-part-only
            // move to 
            // convert datetime to a TimeSpan
            var now = dateTime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        /// <summary>
        ///     Converts the javascript time (long as string) to date time.
        /// </summary>
        /// <param name="time">The time in miliseconds from 1970.</param>
        /// <returns></returns>
        public static DateTime JavaScriptTimeToDateTime(string time)
        {
            if (time == null || time.Equals("null"))
                return DateTime.Now.ToLocalTime();
            var javaTime = long.Parse(time) * 10000;
            var x = new DateTime(new TimeSpan(new DateTime(1970, 1, 1, 0, 0, 0).Ticks + javaTime).Ticks);
            return x.ToLocalTime();
        }

        /// <summary>
        ///     Converts the javascript time (long as string) to date time.
        /// </summary>
        /// <param name="time">The time in miliseconds from 1970.</param>
        /// <returns></returns>
        public static long? DateTimeToJavaScriptTime(DateTime? time)
        {
            if (time == null)
                return null;
            return long.Parse(
                new TimeSpan(time.Value.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks)
                    .TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }
    }
}