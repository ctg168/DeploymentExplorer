using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Jet.Framework.Utility
{
    public class DebugHelper : IDisposable
    {
        private DateTime StartTime;
        private string Title = "";
        private string Info = "";
        private Action<string> OnDispose = null;

        public DebugHelper(string title = "", string info = "", bool isWriteStart = false)
        {
            this.StartTime = DateTime.Now;
            this.Title = title;
            this.Info = info;
            if (isWriteStart)
                Debug.WriteLine(string.Format("{0}:Start", this.Title));
        }

        public DebugHelper(Action<string> onDispose)
        {
            this.StartTime = DateTime.Now;
            this.OnDispose = onDispose;
        }

        public static string GetTimeSpanStr(TimeSpan ts)
        {
            if (ts.Hours > 0)
                return string.Format("{0}小时{1}分{2}秒{3}毫秒", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            else if (ts.Minutes > 0)
                return string.Format("{0}分{1}秒{2}毫秒", ts.Minutes, ts.Seconds, ts.Milliseconds);
            else
                return string.Format("{0}秒{1}毫秒", ts.Seconds, ts.Milliseconds);
        }


        public static void WriteLine(string title, TimeSpan ts, string info = "")
        {
            string strTemp = title;
            if (!string.IsNullOrEmpty(info))
                strTemp = strTemp.PadRight(20, ' ') + info;

            if (ts.Hours > 0)
                Debug.WriteLine("{0}:{1}:{2}:{3}  →  {4}", ts.Hours.ToString().PadLeft(2, '0'), ts.Minutes.ToString().PadLeft(2, '0'), ts.Seconds.ToString().PadLeft(2, '0'), ts.Milliseconds.ToString().PadLeft(3, '0'), strTemp);
            else if (ts.Minutes > 0)
                Debug.WriteLine("{0}:{1}:{2}  →  {3}", ts.Minutes.ToString().PadLeft(2, '0'), ts.Seconds.ToString().PadLeft(2, '0'), ts.Milliseconds.ToString().PadLeft(3, '0'), strTemp);
            else
                Debug.WriteLine("{0}:{1}  →  {2}", ts.Seconds.ToString().PadLeft(2, '0'), ts.Milliseconds.ToString().PadLeft(3, '0'), strTemp);
        }

        public static void WriteLine(string title, string info = "")
        {
            DateTime now = DateTime.Now;

            string strTemp = title;
            if (!string.IsNullOrEmpty(info))
            {
                strTemp = strTemp.PadRight(20, ' ') + info;
            }


            Debug.WriteLine("【{0}:{1}:{2}】  →  {3}"
                            , now.Hour.ToString().PadLeft(2, '0')
                            , now.Minute.ToString().PadLeft(2, '0')
                            , now.Second.ToString().PadLeft(2, '0'),
                            strTemp);

        }

        public void Dispose()
        {
            TimeSpan ts = DateTime.Now - this.StartTime;
            if (this.OnDispose == null)
                DebugHelper.WriteLine(this.Title, ts, this.Info);
            else
                this.OnDispose(DebugHelper.GetTimeSpanStr(ts));
        }
    }
}
