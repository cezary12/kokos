using System;

namespace kokos.Communication.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly long UnixEpochMilliseconds = new DateTime(1970, 1, 1).Ticks / TimeSpan.TicksPerMillisecond;
        public static long ToUnixMilliseconds(this DateTime dt)
        {
            return dt.ToUniversalTime().Ticks / TimeSpan.TicksPerMillisecond - UnixEpochMilliseconds;
        }

        public static DateTime FromUnixMiliseconds(this long unixMiliseconds)
        {
            return new DateTime((UnixEpochMilliseconds + unixMiliseconds) * TimeSpan.TicksPerMillisecond, DateTimeKind.Utc).ToLocalTime();
        }
    }
}
