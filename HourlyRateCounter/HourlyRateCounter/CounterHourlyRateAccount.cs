using System;
using System.Collections.Generic;
using System.Text;

namespace HourlyRateCounter
{
    internal class CounterHourlyRateAccount
    {
        public static double CountsShiftTime(DateTime startWorkShift, DateTime endWorkShift)
        {
            //Этот класс будет потом взаимодействовать
            //Тут мы получаем кол-во часов за смену, с округлением нецелого числа до двужх знаков после ","
            double workingShiftTime = Math.Round((endWorkShift - startWorkShift).TotalHours, 2);
            return workingShiftTime;
        }
    }
}
