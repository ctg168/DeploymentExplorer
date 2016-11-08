using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Jet.Framework.Utility
{
    public enum PeriodEnum
    {
        /// <summary>
        /// 重置
        /// </summary>
        [AttachData("重置")]
        Replace,

        /// <summary>
        /// 昨天
        /// </summary>
        [AttachData("昨天")]
        Yesterday,

        /// <summary>
        /// 昨天之前
        /// </summary>
        [AttachData("昨天之前")]
        BeforeYesterday,

        /// <summary>
        /// 今天
        /// </summary>
        [AttachData("今天")]
        Today,

        /// <summary>
        /// 今天之前
        /// </summary>
        [AttachData("今天之前")]
        BeforeToday,

        /// <summary>
        /// 本周
        /// </summary>
        [AttachData("本周")]
        ThisWeek,

        /// <summary>
        /// 本周之前
        /// </summary>
        [AttachData("本周之前")]
        UntilThisWeek,

        /// <summary>
        /// 上周
        /// </summary>
        [AttachData("上周")]
        LastWeek,

        /// <summary>
        /// 上周之前
        /// </summary>
        [AttachData("上周之前")]
        BeforeLastWeek,

        /// <summary>
        /// 本月
        /// </summary>
        [AttachData("本月")]
        ThisMonth,

        /// <summary>
        /// 本月之前
        /// </summary>
        [AttachData("本月之前")]
        BeforeThisMonth,


        /// <summary>
        /// 下月
        /// </summary>
        [AttachData("下月")]
        NextMonth,

        /// <summary>
        /// 本季
        /// </summary>
        [AttachData("本季")]
        ThisSeason,

        /// <summary>
        /// 下季
        /// </summary>
        [AttachData("下季")]
        NextSeason,


        /// <summary>
        /// 上半年
        /// </summary>
        [AttachData("上半年")]
        FristHalfYear,

        /// <summary>
        /// 下半年
        /// </summary>
        [AttachData("下半年")]
        SecondHalfYear,


        /// <summary>
        /// 上月
        /// </summary>
        [AttachData("上月")]
        LastMonth,

        /// <summary>
        /// 去年
        /// </summary>
        [AttachData("去年")]
        LastYear,

        /// <summary>
        /// 去年或更早
        /// </summary>
        [AttachData("去年或更早")]
        LastYearOrSooner,

        /// <summary>
        /// 今年
        /// </summary>
        [AttachData("今年")]
        ThisYear,

        /// <summary>
        /// 明年
        /// </summary>
        [AttachData("明年")]
        NextYear,

        /// <summary>
        /// 明天
        /// </summary>
        [AttachData("明天")]
        Tomorrow,

        /// <summary>
        /// 今天之后
        /// </summary>
        [AttachData("今天之后")]
        FromThisDay,
        /// <summary>
        /// 一年内
        /// </summary>
        [AttachData("一年内")]
        OneYear,
        /// <summary>
        /// 两年内
        /// </summary>
        [AttachData("两年内")]
        TwoYear,
        /// <summary>
        /// 三年内
        /// </summary>
        [AttachData("三年内")]
        ThreeYear,
        /// <summary>
        /// 三年以上
        /// </summary>
        [AttachData("三年以上")]
        GreaterThreeYear,
        /// <summary>
        /// 五年内
        /// </summary>
        [AttachData("五年内")]
        FiveYear,
        /// <summary>
        /// 五年以上
        /// </summary>
        [AttachData("五年以上")]
        GreaterFiveYear
    }

    public static class DateTimeHelper
    {

        public static string ToTimeSpanString(DateTime startTime, DateTime endTime)
        {
            TimeSpan span = endTime - startTime;

            StringBuilder builder = new StringBuilder();

            if (span.Days > 0)
            {
                builder.AppendFormat("{0}天", span.Days);
            }
            else if (span.Hours > 0)
            {
                builder.AppendFormat("{0}小时", span.Hours);
            }
            else if (span.Minutes > 0)
            {
                builder.AppendFormat("{0}分钟", span.Minutes);
            }

            return builder.ToString();
        }

        public static string ToSqlDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string ToSqlTimeString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH：mm：ss");
        }

        public static bool IsHaveSameTimeSpan(DateTime dtStart1, DateTime dtEnd1, DateTime dtStart2, DateTime dtEnd2)
        {
            return dtStart1 <= dtEnd2 && dtEnd1 >= dtStart2;
        }

        public static int GetDaysInMonth(int year, int month)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            return firstDay.AddMonths(1).AddDays(-1).Day;
        }

        /// <summary>
        /// 获取该时间当前月的第一天
        /// </summary>
        public static DateTime GetMonthFirstDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// 获取该时间当前月的最后一天
        /// </summary>
        public static DateTime GetMonthLastDay(DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, 1);
            return date.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获取两个时间之间的间隔(分钟)
        /// </summary>
        /// <param name="fisrtDateTime"></param>
        /// <param name="lastDateTime"></param>
        /// <returns></returns>
        public static int DateTimeDiffAsMinutes(DateTime? firstDateTime, DateTime? lastDateTime)
        {
            if (firstDateTime == null || lastDateTime == null)
            {
                return 0;
            }

            TimeSpan timespan = firstDateTime.Value - lastDateTime.Value;

            return Math.Abs((int)timespan.TotalMinutes);
        }


        /// <summary>
        /// 获取两个时间之间的间隔(秒)
        /// </summary>
        /// <param name="fisrtDateTime"></param>
        /// <param name="lastDateTime"></param>
        /// <returns></returns>
        public static int DateTimeDiffAsSeconds(DateTime? firstDateTime, DateTime? lastDateTime)
        {
            if (firstDateTime == null || lastDateTime == null)
            {
                return 0;
            }

            TimeSpan timespan = firstDateTime.Value - lastDateTime.Value;

            return Math.Abs((int)timespan.TotalSeconds);
        }


        /// <summary>
        /// 获取指定时间段内的所有日期的集合,精确到天,时间按ASC排序
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            TimeSpan span = (endDate.Date - startDate.Date);
            int range = span.Days;

            List<DateTime> dtList = new List<DateTime>();

            for (int i = 0; i <= Math.Abs(range); i++)
            {
                if (range < 0)
                {
                    dtList.Add(startDate.AddDays((0 - i)));
                }
                else
                {
                    dtList.Add(startDate.AddDays((i)));
                }
            }

            return dtList.OrderBy(s => s).ToList();
        }

        /// <summary>
        /// 获取某年某月的开始时间和结束时间(从当前月的00:00:00-月末的23:59:59)
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        public static void GetMonthSpan(int year, int month, out DateTime beginDateTime, out DateTime endDateTime)
        {
            beginDateTime = new DateTime(year, month, 1);
            endDateTime = beginDateTime.AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取某年某一季度的开始时间和结束时间(从当前季度的00:00:00-该季度末的23:59:59)
        /// 该方法接收一个月份，自动计算季度
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        public static void GetQuarterSpan(int year, int month, out DateTime beginDateTime, out DateTime endDateTime)
        {
            DateTime dt = new DateTime(year, month, 1);

            beginDateTime = dt.AddMonths(0 - (dt.Month - 1) % 3).AddDays(1 - dt.Day);
            endDateTime = beginDateTime.AddMonths(3).AddSeconds(-1);
        }

        /// <summary>
        /// 获取某一年的开始时间和结束时间(从当前年的00:00:00-年末的23:59:59)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        public static void GetYearSpan(int year, out DateTime beginDateTime, out DateTime endDateTime)
        {
            beginDateTime = new DateTime(year, 1, 1);
            endDateTime = beginDateTime.AddYears(1).AddSeconds(-1);
        }


        /// <summary>
        /// 获取某一年的某一周的日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static List<DateTime> GetCurrentWeekRang(int year, int week)
        {
            DateTime firstDate;
            DateTime lastDate;

            List<DateTime> dates = new List<DateTime>();

            if (GetDaysOfWeeks(year, week, CalendarWeekRule.FirstFullWeek, out firstDate, out lastDate))
            {
                dates.Add(firstDate);
                dates.Add(firstDate.AddDays(1));
                dates.Add(firstDate.AddDays(2));
                dates.Add(firstDate.AddDays(3));
                dates.Add(firstDate.AddDays(4));
                dates.Add(firstDate.AddDays(5));
                dates.Add(firstDate.AddDays(6));
            }

            return dates;
        }

        /// <summary>
        /// 获取一年有多少个周
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetWeekAmount(int year)
        {
            DateTime date = new DateTime(year, 12, 31);

            GregorianCalendar calendar = new GregorianCalendar();

            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }


        /// <summary>
        /// 获取某一年某一周的开始时间和结束时间
        /// </summary>
        /// <param name="year"></param>
        /// <param name="week"></param>
        /// <param name="weekrule"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static bool GetDaysOfWeeks(int year, int week, CalendarWeekRule weekrule, out DateTime first, out DateTime last)
        {
            first = DateTime.MinValue;
            last = DateTime.MinValue;

            if (year < 1 || year > 9999 || week < 1 || week > 53) { return false; }


            DateTime firstCurr = new DateTime(year, 1, 1);
            DateTime firstNext = new DateTime(year + 1, 1, 1);


            int dayOfWeekFirst = (int)firstCurr.DayOfWeek;
            if (dayOfWeekFirst == 0) { dayOfWeekFirst = 7; }


            first = firstCurr.AddDays((week - 1) * 7 - dayOfWeekFirst + 1);

            if (first.Year < year)
            {
                switch (weekrule)
                {
                    case CalendarWeekRule.FirstDay:
                        first = firstCurr;
                        break;
                    case CalendarWeekRule.FirstFourDayWeek:
                        first = first.AddDays(7);
                        break;
                    case CalendarWeekRule.FirstFullWeek:
                        if (firstCurr.Subtract(first).Days > 3)
                        {
                            first = first.AddDays(7);
                        }
                        break;
                    default:
                        break;
                }
            }


            last = first.AddDays(7).AddSeconds(-1);

            if (last.Year > year)
            {
                switch (weekrule)
                {
                    case CalendarWeekRule.FirstDay:
                        last = firstNext.AddSeconds(-1);
                        break;
                    case CalendarWeekRule.FirstFullWeek:
                        break;
                    case CalendarWeekRule.FirstFourDayWeek:
                        if (firstNext.Subtract(first).Days < 4)
                        {
                            first = first.AddDays(-7);
                            last = last.AddDays(-7);
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        public static void FillPeriodByPeriodEnumIntVal(int periodEnumIntVal, out DateTime? startTime, out DateTime? endTime)
        {
            DateTime now = DateTime.Now.Date;

            int weekIntVal = (int)now.DayOfWeek;
            if (0 == weekIntVal)
            {
                weekIntVal = 7;
            }

            switch (periodEnumIntVal)
            {
                case (int)PeriodEnum.Yesterday:
                    {
                        startTime = now.AddDays(-1);
                        endTime = now.AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.BeforeYesterday:
                    {
                        startTime = null;
                        endTime = now.AddDays(-1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.Today:
                    {
                        startTime = now;
                        endTime = now.AddDays(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.BeforeToday:
                    {
                        startTime = null;
                        endTime = now.AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.ThisWeek:
                    {
                        DateTime firstDayOfWeek = now.AddDays(1 - weekIntVal);
                        startTime = firstDayOfWeek;
                        endTime = firstDayOfWeek.AddDays(7).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.UntilThisWeek:
                    {
                        startTime = null;
                        endTime = now.AddDays(1 - weekIntVal).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.LastWeek:
                    {
                        DateTime firstDayOfPreWeek = now.AddDays(1 - weekIntVal - 7);
                        startTime = firstDayOfPreWeek;
                        endTime = firstDayOfPreWeek.AddDays(7).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.BeforeLastWeek:
                    {
                        startTime = null;
                        endTime = now.AddDays(1 - weekIntVal - 7).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.ThisMonth:
                    {
                        DateTime firstDayOfMonth = now.AddDays(1 - now.Day);
                        startTime = firstDayOfMonth;
                        endTime = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.BeforeThisMonth:
                    {
                        startTime = null;
                        endTime = now.AddDays(1 - now.Day).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.LastMonth:
                    {
                        //DateTime firstDayOfPreMonth = now.AddDays(1 - now.Day).AddMonths(-1);
                        //startTime = firstDayOfPreMonth;
                        //endTime = now.AddDays(1 - now.Day).AddSeconds(-1);
                        var tempTime = now.AddMonths(-1);
                        startTime = new DateTime(tempTime.Year, tempTime.Month, 1);
                        endTime = startTime.Value.AddMonths(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.LastYear:
                    {
                        DateTime last = new DateTime(now.Year - 1, 1, 1);

                        startTime = last;
                        endTime = last.AddYears(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.LastYearOrSooner:
                    {
                        DateTime thisdata = new DateTime(now.Year, 1, 1);

                        startTime = null;
                        endTime = thisdata.AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.ThisYear:
                    {
                        DateTime thisdata = new DateTime(now.Year, 1, 1);

                        startTime = thisdata;
                        endTime = thisdata.AddYears(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.Tomorrow:
                    {
                        startTime = now.AddDays(1);
                        endTime = now.AddDays(2).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.FromThisDay:
                    {
                        startTime = now;
                        endTime = null;
                        break;
                    }
                case (int)PeriodEnum.NextMonth:
                    {
                        var tempTime = now.AddMonths(1);
                        startTime = new DateTime(tempTime.Year, tempTime.Month, 1);
                        endTime = startTime.Value.AddMonths(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.ThisSeason:
                    {
                        var date = now.AddMonths(0 - (now.Month - 1) % 3);
                        startTime = new DateTime(date.Year, date.Month, 1);
                        endTime = startTime.Value.AddMonths(3).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.NextSeason:
                    {
                        var date = now.AddMonths(3 - (now.Month - 1) % 3);
                        startTime = new DateTime(date.Year, date.Month, 1);
                        endTime = startTime.Value.AddMonths(3).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.FristHalfYear:
                    {
                        startTime = new DateTime(now.Year, 1, 1);
                        endTime = startTime.Value.AddMonths(6).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.SecondHalfYear:
                    {
                        startTime = new DateTime(now.Year, 7, 1);
                        endTime = startTime.Value.AddMonths(6).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.NextYear:
                    {
                        startTime = new DateTime(now.Year + 1, 1, 1);
                        endTime = startTime.Value.AddYears(1).AddSeconds(-1);
                        break;
                    }
                case (int)PeriodEnum.OneYear:
                    {
                        startTime = now.AddYears(-1);
                        endTime = now;
                        break;
                    }
                case (int)PeriodEnum.TwoYear:
                    {
                        startTime = now.AddYears(-2);
                        endTime = now;
                        break;
                    }
                case (int)PeriodEnum.ThreeYear:
                    {
                        startTime = now.AddYears(-3);
                        endTime = now;
                        break;
                    }
                case (int)PeriodEnum.GreaterThreeYear:
                    {
                        startTime = null;
                        endTime = now.AddYears(-3);
                        break;
                    }
                case (int)PeriodEnum.FiveYear:
                    {
                        startTime = now.AddYears(-5);
                        endTime = now;
                        break;
                    }
                case (int)PeriodEnum.GreaterFiveYear:
                    {
                        startTime = null;
                        endTime = now.AddYears(-5);
                        break;
                    }
                default:
                    {
                        startTime = null;
                        endTime = null;
                        break;
                    }
            }
        }
    }
}
