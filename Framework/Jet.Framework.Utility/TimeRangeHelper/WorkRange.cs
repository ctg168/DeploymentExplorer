using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class WorkRange
    {
        public int TheYear { get; private set; }

        public TimeRange TimeRange { get; private set; }

        public List<Month> MonthList { get; private set; }

        public List<Quarter> QuarterList { get; private set; }

        public List<HalfYear> HalfyearList { get; private set; }

        public WorkRange(DateTime startDatetime, DateTime endDateTime)
        {
            if (startDatetime.Year != endDateTime.Year) throw new Exception("开始时间和结束时间必须在同一年度!");
            if (startDatetime > endDateTime) throw new Exception("结束时间必须大于开始时间!");

            this.TheYear = startDatetime.Year;
            this.TimeRange = new TimeRange(startDatetime, new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, 23, 59, 59));
            InitQuarter();
            InitHalfYear();
            InitMonth();
        }

        private void InitQuarter()
        {
            QuarterList = new List<Quarter>();
            QuarterList.Add(new Quarter(TheYear, 1));
            QuarterList.Add(new Quarter(TheYear, 2));
            QuarterList.Add(new Quarter(TheYear, 3));
            QuarterList.Add(new Quarter(TheYear, 4));
        }

        private void InitHalfYear()
        {
            HalfyearList = new List<HalfYear>();
            HalfyearList.Add(new HalfYear(TheYear, 1));
            HalfyearList.Add(new HalfYear(TheYear, 2));
        }

        private void InitMonth()
        {
            MonthList = new List<Month>();
            MonthList.Add(new Month(TheYear, 1));
            MonthList.Add(new Month(TheYear, 2));
            MonthList.Add(new Month(TheYear, 3));
            MonthList.Add(new Month(TheYear, 4));
            MonthList.Add(new Month(TheYear, 5));
            MonthList.Add(new Month(TheYear, 6));
            MonthList.Add(new Month(TheYear, 7));
            MonthList.Add(new Month(TheYear, 8));
            MonthList.Add(new Month(TheYear, 9));
            MonthList.Add(new Month(TheYear, 10));
            MonthList.Add(new Month(TheYear, 11));
            MonthList.Add(new Month(TheYear, 12));
        }

        /// <summary>
        /// 根据一个日期得到所在的季度
        /// </summary>
        /// <param name="theTime"></param>
        /// <returns></returns>
        public Quarter GetQuarterOfDateTime(DateTime theTime)
        {
            return QuarterList.First(r => r.TimeRange.IsInRange(theTime));
        }

        /// <summary>
        /// 根据日期得到所在的半年度
        /// </summary>
        /// <param name="theTime"></param>
        /// <returns></returns>
        public HalfYear GetHalfYearOfDateTime(DateTime theTime)
        {
            return HalfyearList.First(r => r.TimeRange.IsInRange(theTime));
        }

        /// <summary>
        /// 根据日期得到所在的月份
        /// </summary>
        /// <param name="theTime"></param>
        /// <returns></returns>
        public Month GetMonthOfDateTime(DateTime theTime)
        {
            return MonthList.First(r => r.TimeRange.IsInRange(theTime));
        }

        /// <summary>
        /// 得到本年度指定时间段内可用的月份列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, TimeRange> GetWorkedMonthList()
        {
            Dictionary<int, TimeRange> mDict = new Dictionary<int, TimeRange>();
            if (TimeRange.StartTime.Month == TimeRange.EndTime.Month) //in the same montJet.Framework.
            {
                mDict.Add(TimeRange.StartTime.Month, TimeRange);
                return mDict;
            }

            //the first month
            mDict.Add(TimeRange.StartTime.Month, // key
                        new TimeRange(TimeRange.StartTime, //start time
                            GetMonthOfDateTime(TimeRange.StartTime).TimeRange.EndTime
                            ) // end time
                    );

            //between first month and last montJet.Framework.
            foreach (var monthItem in MonthList)
            {
                if (monthItem.Index > TimeRange.StartTime.Month &&
                    monthItem.Index < TimeRange.EndTime.Month)
                {
                    mDict.Add(monthItem.Index, monthItem.TimeRange);
                }
            }

            //the last month
            mDict.Add(TimeRange.EndTime.Month, // key
                        new TimeRange(GetMonthOfDateTime(TimeRange.EndTime).TimeRange.StartTime,
                            TimeRange.EndTime) // end time
                    );

            return mDict;
        }

        /// <summary>
        /// 得到本年度指定时间段内可用的季度列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, TimeRange> GetWorkedQuarterList()
        {
            Dictionary<int, TimeRange> qDict = new Dictionary<int, TimeRange>();

            if (GetQuarterOfDateTime(TimeRange.StartTime).Index == GetQuarterOfDateTime(TimeRange.EndTime).Index) //in the same quarter.
            {
                qDict.Add(GetQuarterOfDateTime(TimeRange.StartTime).Index, TimeRange);
                return qDict;
            }

            //the first quarter in the time range
            qDict.Add(GetQuarterOfDateTime(TimeRange.StartTime).Index, // key
                        new TimeRange(TimeRange.StartTime, //start time
                        GetQuarterOfDateTime(TimeRange.StartTime).TimeRange.EndTime
                        ) // end time
                    );

            //between first quarter and last quarter.
            foreach (var qItem in QuarterList)
            {
                if (qItem.Index > GetQuarterOfDateTime(TimeRange.StartTime).Index &&
                    qItem.Index < GetQuarterOfDateTime(TimeRange.EndTime).Index)
                {
                    qDict.Add(qItem.Index, qItem.TimeRange);
                }
            }

            //the last quarter
            qDict.Add(GetQuarterOfDateTime(TimeRange.EndTime).Index, // key
                        new TimeRange(GetQuarterOfDateTime(TimeRange.EndTime).TimeRange.StartTime,
                            TimeRange.EndTime) // end time
                    );

            return qDict;
        }

        /// <summary>
        /// 得到本年度指定时间段内可用的半年度列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, TimeRange> GetWorkedHalfYearList()
        {
            Dictionary<int, TimeRange> qDict = new Dictionary<int, TimeRange>();

            if (GetHalfYearOfDateTime(TimeRange.StartTime).Index == GetHalfYearOfDateTime(TimeRange.EndTime).Index) //in the same quarter.
            {
                qDict.Add(GetHalfYearOfDateTime(TimeRange.StartTime).Index, TimeRange);
                return qDict;
            }

            //the first half year
            qDict.Add(1, new TimeRange(TimeRange.StartTime, HalfyearList[0].TimeRange.EndTime));

            //the last half year 
            qDict.Add(2, new TimeRange(HalfyearList[1].TimeRange.StartTime, TimeRange.EndTime));

            return qDict;
        }

        /// <summary>
        /// 得到指定月份的有效天数
        /// 例如，工作年度初始化为 1.15 - 3.18时，给定1月份，则返回1.15-1.31中的有效天数，应该是17天。
        /// </summary>
        /// <param name="timeRangeType">时间段类型</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public int GetWorkMonthDayofPeriod(int index)
        {
            if (index < 1 || index > 12) throw new Exception("month index must between 1 and 12");

            if (GetMonthOfDateTime(TimeRange.StartTime).Index == GetMonthOfDateTime(TimeRange.EndTime).Index)
            {
                return (TimeRange.StartTime - TimeRange.EndTime).Days;
            }

            if (index == TimeRange.StartTime.Month)
                return DateTimeHelper.GetDaysInMonth(this.TheYear, TimeRange.StartTime.Month) - TimeRange.StartTime.Day + 1;

            if (index == TimeRange.EndTime.Month)
                return TimeRange.EndTime.Day;

            if (index >= TimeRange.StartTime.Month && index <= TimeRange.EndTime.Month)
                return DateTimeHelper.GetDaysInMonth(this.TheYear, index);

            return 0;
        }

        /// <summary>
        /// 得到指定季度的有效天数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetWorkQuarterDayofPeriod(int index)
        {
            if (index < 1 || index > 4) throw new Exception("quarter index must between 1 and 4");

            if (GetQuarterOfDateTime(TimeRange.StartTime).Index == GetQuarterOfDateTime(TimeRange.EndTime).Index) //同一季度
            {
                return (TimeRange.StartTime - TimeRange.EndTime).Days;
            }

            if (index == GetQuarterOfDateTime(TimeRange.StartTime).Index) //在开始季度
            {
                return (GetQuarterOfDateTime(TimeRange.StartTime).TimeRange.EndTime - TimeRange.StartTime).Days;
            }

            if (index == GetQuarterOfDateTime(TimeRange.EndTime).Index) //在结束季度
            {
                return (TimeRange.EndTime - GetQuarterOfDateTime(TimeRange.EndTime).TimeRange.StartTime).Days;
            }

            //其他季度
            if (index > GetQuarterOfDateTime(TimeRange.StartTime).Index &&
                index < GetQuarterOfDateTime(TimeRange.EndTime).Index) //范围之内
            {
                return (QuarterList[index - 1].TimeRange.EndTime - QuarterList[index - 1].TimeRange.StartTime).Days;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 得到指定半年度的有效天数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetWorkHalfyearDayofPeriod(int index)
        {
            if (index < 1 || index > 2) throw new Exception("half year index must between 1 and 2");

            if (GetHalfYearOfDateTime(TimeRange.StartTime).Index == GetHalfYearOfDateTime(TimeRange.EndTime).Index) //同一个半年
            {
                return (TimeRange.StartTime - TimeRange.EndTime).Days;
            }

            if (index == GetHalfYearOfDateTime(TimeRange.StartTime).Index) //上半年
            {
                return (HalfyearList[0].TimeRange.EndTime - TimeRange.StartTime).Days;
            }
            else //下半年
            {
                return (TimeRange.EndTime - HalfyearList[1].TimeRange.StartTime).Days;
            }
        }
    }
}
