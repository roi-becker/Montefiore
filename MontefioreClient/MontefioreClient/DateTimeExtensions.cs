using System;

namespace MontefioreClient
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts the DateTime to Unix time (ticks since 1970-1-1)
        /// </summary>
        /// <remarks>
        /// Make sure the DateTimeKind is correctly configured, so that UTC conversion will work properly.
        /// </remarks>
        public static Int64 ToUnixTime(this DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new InvalidOperationException("Converting to unix time should be done only of UTC timezone dates.");
            }
            return Convert.ToInt64((dateTime.ToUniversalTime() - epoch).TotalMilliseconds);
        }

        public static DateTime FromUnixTime(Int64 dateTime)
        {
            return epoch.AddMilliseconds(dateTime);
        }
    }
}
