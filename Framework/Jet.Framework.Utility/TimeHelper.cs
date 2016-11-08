using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class TimeHelper
    {
        // GetDaysInMonth
        public static int GetDaysInMonth(int year, int month)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            return firstDay.AddMonths(1).AddDays(-1).Day;
        }
    }
}
