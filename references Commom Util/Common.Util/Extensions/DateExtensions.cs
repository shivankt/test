using System;

namespace Common.Util
{
    public static class DateExtensions
    {
        public static string UtilToISOFormat(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ss.fffzzzz");
        }

        public static string UtilToMySqlFormat(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Add the current time in selected date
        public static DateTime UtilToAddTimeInDate(this DateTime dt)
        {
            return Convert.ToDateTime(dt.ToString("MM/dd/yyyy") + " " + DateTime.Now.ToString("hh:mm:ss"));
        }
    }
}
