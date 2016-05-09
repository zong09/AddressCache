using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressCache
{
    public enum TimeUnit
    {
        DAYS ,
        HOURS ,
        MICROSECONDS,
        MILLISECONDS,
        MINUTES,
        NANOSECONDS,
        SECONDS ,
    }

    static class TimeUnitMethods
    {

        public static double ConvertToMilliseconds(this TimeUnit t)
        {
            switch (t)
            {
                case TimeUnit.DAYS:
                    return 86400000;
                case TimeUnit.HOURS:
                    return 3600000;
                case TimeUnit.MICROSECONDS:
                    return 0.001;
                case TimeUnit.MILLISECONDS:
                    return 1;
                case TimeUnit.MINUTES:
                    return 60000;
                case TimeUnit.NANOSECONDS:
                    return 0.000001;
                case TimeUnit.SECONDS:
                    return 1000;
                default:
                    return 1;
            }
        }
    }
}