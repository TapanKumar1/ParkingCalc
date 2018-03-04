using System;

namespace Parking.Utils
{
	public static class DateTimeExtensions
	{
		public static bool IsWeekday(this DateTime dt)
		{
			return dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday;
		}
	}
}