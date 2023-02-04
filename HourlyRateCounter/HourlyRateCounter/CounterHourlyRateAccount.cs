using System;
using System.Collections.Generic;
using System.Text;

namespace HourlyRateCounter
{
    internal class CounterHourlyRateAccount
    {
        public static double CountsShiftTime(DateTime startWorkShift, DateTime endWorkShift)
        {
            double workingShiftTime = Math.Round((endWorkShift - startWorkShift).TotalHours, 2);
            return workingShiftTime;
        }
    }
}
