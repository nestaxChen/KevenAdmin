using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime time, bool isMinlliseconds = true)
        {
            TimeSpan ts = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            if (isMinlliseconds)
                return Convert.ToInt64(ts.TotalMilliseconds);
            else
                return Convert.ToInt64(ts.TotalSeconds);

        }

        public static DateTime ToDateTimeFromTimeStamp(this long timestamp)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newdate = startTime.AddSeconds(timestamp);
            newdate = newdate.ToLocalTime();
            return newdate;
        }

        //public static string LeftTime(this DateTime date, string format = "ddHHmmss")
        //{
        //    return Keven.Common.DateTimeHelper.LeftTime(date, format);
        //}
    }
}
