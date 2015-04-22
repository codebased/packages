using System;

namespace Ap.Common.Extensions
{
    public static class Dates
    {
        public static DateTime GetMonday(this DateTime time)
        {
            if (time.DayOfWeek != DayOfWeek.Monday)
                return GetMonday(time.AddDays(-1)); //Recursive call

            return time;
        }

        public static DateTime GetSunday(this DateTime time)
        {
            if (time.DayOfWeek != DayOfWeek.Sunday)
            {
                return GetSunday(time.AddDays(1)); //Recursive call
            }

            return time;
        }
    }
}
