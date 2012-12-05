using System;
using System.Reflection;
namespace Common.Util
{
    public static class DateTimeExtensions
    {
        public static string ToUtcISOString(this DateTime dt) 
        {
            return dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff") + "+00:00";
        }

        static string GetOffsetString(TimeZoneInfo tzInfo)
        {
            return string.Format("{0}{1}:{2}", (tzInfo.BaseUtcOffset.Hours < 0 ? string.Empty : "+"), tzInfo.BaseUtcOffset.Hours, tzInfo.BaseUtcOffset.Minutes);
        }

        public static string ToUtcISOString(this DateTime dt, string sourceTzID)
        {
            if (dt != null
                && !string.IsNullOrEmpty(sourceTzID)
                )
            {
                TimeZoneInfo tzSource = TimeZoneInfo.FindSystemTimeZoneById(sourceTzID);
                TimeSpan tsSource = new TimeSpan(tzSource.BaseUtcOffset.Hours, tzSource.BaseUtcOffset.Minutes, tzSource.BaseUtcOffset.Seconds);

                DateTimeOffset sourceTime, baseTime;
                sourceTime = new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, tsSource);//source time
                baseTime = sourceTime.ToOffset(TimeSpan.Zero);//offset 0 time
                return baseTime.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "+00:00";
            }
            return dt.ToUtcISOString();
        }

        public static string FromUtcISOString(this string ISODate, string destTimeZoneID)
        {
            if (!string.IsNullOrEmpty(destTimeZoneID)
                )
            {
                TimeZoneInfo tzDest = TimeZoneInfo.FindSystemTimeZoneById(destTimeZoneID);
                TimeSpan tsDest = new TimeSpan(tzDest.BaseUtcOffset.Hours, tzDest.BaseUtcOffset.Minutes, tzDest.BaseUtcOffset.Seconds);

                DateTimeOffset sourceTime, destTime;
                sourceTime = DateTimeOffset.Parse(ISODate);//source time
                destTime = sourceTime.ToOffset(tsDest);//dest time

                return destTime.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + GetOffsetString(tzDest);
            }
            return ISODate;
        }

        public static string GetTimeOffSetFromTimeZoneID(this string tzID)
        {
            if (!string.IsNullOrEmpty(tzID))
            {
                TimeZoneInfo tzDest = TimeZoneInfo.FindSystemTimeZoneById(tzID);
                int h = tzDest.BaseUtcOffset.Hours;
                int m = tzDest.BaseUtcOffset.Minutes;
                if (h < 0)
                    return string.Format("{0}:{1}", h, m);
                else
                    return string.Format("+{0}:{1}", h, m);

            }
            return string.Empty;
        }
    }
}
