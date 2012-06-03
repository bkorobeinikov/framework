using System;
using System.Globalization;

namespace Bobasoft
{
    public static class DateTimeExtensions
    {
        //======================================================
        #region _Public methods_

        public static string GetFormatedTicks(this DateTime dateTime)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:D19}", dateTime.Ticks);
        }

        public static string GetFormattedInvertedTicks(this DateTime dateTime)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:D19}", dateTime.Ticks - DateTime.MaxValue.Ticks);
        }

        public static DateTime AsUtc(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static DateTime SubstractToHours(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, dateTime.Kind);
        }

        public static DateTime SubstractToMinutes(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);
        }

        public static DateTime SubstractToSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
        }

        public static DateTime Normalize(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year,
                                dateTime.Month,
                                dateTime.Day,
                                dateTime.Hour,
                                dateTime.Minute,
                                dateTime.Second,
                                dateTime.Millisecond,
                                dateTime.Kind);
        }

        public static double TotalMilliseconds(this DateTime dateTime)
        {
            return (dateTime - StartDateTime).TotalMilliseconds;
        } 

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private static readonly DateTime StartDateTime = new DateTime(1970, 1, 1);

        #endregion
    }
}