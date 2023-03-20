using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
	public class TimeZone
	{
		public static DateTime GetLocalDateTime()
		{

			TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, infotime);
		}


		public static double GetCurrentTimeStamp()
		{
			var dateTime = GetLocalDateTime();
			dateTime = new DateTime(
									dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),
									dateTime.Kind
									);

			return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}
	}
}
