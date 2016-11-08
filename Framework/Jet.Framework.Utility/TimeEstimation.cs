using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class TimeEstimation
    {
        private DateTime StartTime = DateTime.MinValue;

        public bool IsBegin { get; private set; }

        public void BeginCompute()
        {
            this.StartTime = DateTime.Now;
            this.IsBegin = true;
        }

        public TimeSpan Compute(double complete, double total)
        {
            if (total == 0)
                return TimeSpan.FromMilliseconds(0);

            TimeSpan ts = DateTime.Now - StartTime;
            double estimation = ts.TotalMilliseconds * ((total - complete) / complete);
            return TimeSpan.FromMilliseconds(estimation);
        }

        public static string GetTimeEstimationText(TimeSpan ts)
        {
            if (ts == TimeSpan.MaxValue || ts.Seconds < 0)
                return string.Empty;

            string time = string.Empty;
            if (ts.Hours > 0)
            {
                time = string.Format("{0}小时{1}分", ts.Hours, ts.Minutes);
            }
            else
            {


                if (ts.Seconds >= 55)
                    time = string.Format("{0}分55秒", ts.Minutes);
                else
                    time = string.Format("{0}分{1}秒", ts.Minutes, (ts.Seconds / 5 + 1) * 5);
            }

            return string.Format("估计剩余:{0}", time);
        }
    }
}
