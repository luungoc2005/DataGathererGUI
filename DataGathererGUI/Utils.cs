using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataGathererGUI
{
    public static class Utils
    {
        public static long GetUnixTime(DateTime time)
        {
            return (long)time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime UnixToDateTime(string value)
        {
            try
            {
                var seconds = long.Parse(value);
                return (new DateTime(1970, 1, 1)).AddSeconds(seconds);
            }
            catch
            {
                return new DateTime(1970, 1, 1);
            }
        }

        public static DateTime GetNextDay(DateTime value)
        {
            var retVal = value.AddDays(1);
            while (retVal.DayOfWeek == DayOfWeek.Saturday || retVal.DayOfWeek == DayOfWeek.Sunday)
            {
                retVal = retVal.AddDays(1);
            }
            return retVal;
        }

        public static double GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return calcBusinessDays;
        }
    }
}
