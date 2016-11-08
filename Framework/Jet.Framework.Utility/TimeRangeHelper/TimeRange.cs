using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    /// <summary>
    /// 时间段
    /// </summary>
    public class TimeRange
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeRange(DateTime startTime, DateTime endTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public bool IsInRange(DateTime theTime)
        {
            return theTime >= StartTime && theTime <= EndTime;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", StartTime, EndTime);
        }

    }

    /// <summary>
    /// 月份
    /// </summary>
    public class Month
    {
        public int Index { get; private set; }

        public TimeRange TimeRange { get; private set; }

        public int Year { get; private set; }

        public Month(int year, int monthNumber)
        {
            this.Year = year;
            this.Index = monthNumber;
            this.TimeRange = new TimeRange(new DateTime(year, monthNumber, 1),
                new DateTime(year, monthNumber, DateTimeHelper.GetDaysInMonth(year, monthNumber), 23, 59, 59));
        }
    }

    /// <summary>
    /// 季度
    /// </summary>
    public class Quarter
    {
        public int Index { get; private set; }

        public TimeRange TimeRange { get; private set; }

        public int Year { get; private set; }

        public Quarter(int year, int quarterIndex)
        {
            this.Year = year;
            this.Index = quarterIndex;

            if (quarterIndex == 1)
            {
                TimeRange = new TimeRange(new DateTime(Year, 1, 1), new DateTime(Year, 3, 31, 23, 59, 59));
            }
            else if (quarterIndex == 2)
            {
                TimeRange = new TimeRange(new DateTime(Year, 4, 1), new DateTime(Year, 6, 30, 23, 59, 59));
            }
            else if (quarterIndex == 3)
            {
                TimeRange = new TimeRange(new DateTime(Year, 7, 1), new DateTime(Year, 9, 30, 23, 59, 59));
            }
            else if (quarterIndex == 4)
            {
                TimeRange = new TimeRange(new DateTime(Year, 10, 1), new DateTime(Year, 12, 31, 23, 59, 59));
            }
            else
            {
                throw new Exception("季度索引必须是1到4!");
            }
        }
    }

    /// <summary>
    /// 半年度
    /// </summary>
    public class HalfYear
    {
        public int Index { get; private set; }

        public TimeRange TimeRange { get; private set; }

        public int Year { get; private set; }

        public HalfYear(int year, int halfYearIndex)
        {
            this.Year = year;
            this.Index = halfYearIndex;
            if (halfYearIndex == 1)
            {
                TimeRange = new TimeRange(new DateTime(year, 1, 1), new DateTime(year, 6, 30, 23, 59, 59));
            }
            else if (halfYearIndex == 2)
            {
                TimeRange = new TimeRange(new DateTime(year, 7, 1), new DateTime(year, 12, 31, 23, 59, 59));
            }
            else
            {
                throw new Exception("半年索引必须是1(上半年)和2(下半年)!");
            }
        }
    }
}
