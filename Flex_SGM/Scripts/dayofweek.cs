using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Flex_SGM.Scripts
{
    public class dayofweek
    {
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static int GetIso8601WeekOfYear(DateTime? time)
        {
            if (time!=null) { 
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time.Value);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.Value.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
            else
            {
                return 0;
            }
        }
    }

    public class dtconverter
    {

        /// <summary>
        /// Convierte de Unix Timestamp a Datetime
        /// summary>
        /// <param name="timestamp">Date to convertparam>
        /// <returns>returns>
        public static DateTime FromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        /// <summary>
        /// convierte de DateTime a UNIX Timestamp
        /// summary>
        /// <param name="value">Date to convertparam>
        /// <returns>returns>
        public static double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
        }


    }

}