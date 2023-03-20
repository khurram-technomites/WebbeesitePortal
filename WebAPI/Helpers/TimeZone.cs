using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public static class TimeZone
    {
        public static DateTime ToDubaiDateTime(this DateTime date)
        {
            TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, infotime);
        }

        public static TimeSpan ToDubaiDateTime(this TimeSpan time)
        {
            DateTime datetime = DateTime.UtcNow + time;
            TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(datetime, infotime).TimeOfDay;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

    }
}
