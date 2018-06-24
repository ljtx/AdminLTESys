using System;
using System.Globalization;

namespace Ambiel.Utility.Time
{
    public class TimeHelper
    {
           private static readonly string[] CultureSources = new string[]
	{
		"en-us",
		"zh-cn",
		"ar-iq",
		"de-de"
	};

        public static string DateUTC(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss:fffZ");
        }

        public static string ToGMTString(DateTime dt)
        {
            return dt.ToUniversalTime().ToString("r");
        }

        public static string DateFormatByen_us(DateTime MyDate)
        {
            CultureInfo provider = new CultureInfo("en-us");
            return MyDate.ToString("ddMMMyyyy", provider);
        }

        public static string GetTimeStamp(DateTime time, int length = 13)
        {
            return TimeHelper.ConvertDateTimeToInt(time).ToString().Substring(0, length);
        }

        public static long ConvertDateTimeToInt(DateTime time)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (time.Ticks - dateTime.Ticks) / 10000L;
        }

        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ticks = long.Parse(timeStamp + "0000");
            TimeSpan value = new TimeSpan(ticks);
            return dateTime.Add(value);
        }

        public static DateTime GetDateTimeFrom1970Ticks(long curSeconds)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds((double)curSeconds);
        }

        public static bool IsTime(long time, double interval)
        {
            DateTime dateTimeFrom1970Ticks = TimeHelper.GetDateTimeFrom1970Ticks(time);
            DateTime t = DateTime.Now.AddMinutes(interval);
            DateTime t2 = DateTime.Now.AddMinutes(interval * -1.0);
            return dateTimeFrom1970Ticks > t2 && dateTimeFrom1970Ticks < t;
        }

        public static bool IsTime(string time)
        {
            string timeStamp = TimeHelper.GetTimeStamp(DateTime.Now, 8);
            return timeStamp.Equals(time);
        }
    }
}