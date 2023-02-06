using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace HourlyRateCounter
{
    internal class Statistic
    {
         int salary;            // Тут будем сохранять сумму заработанную за месяц
         int timeHours;       // Тут общее кол-во отработанного времени
        Months1 month;

        public Statistic(int salary, int timeHours, Months1 month)
        {
            this.salary = salary;
            this.timeHours = timeHours;
            this.month = month;
        }
    }
}
