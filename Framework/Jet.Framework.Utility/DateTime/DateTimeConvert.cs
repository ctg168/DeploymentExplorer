using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Jet.Framework.Utility
{
    public class DateTimeConvert
    {
        private static List<string> DateTimeFormatList = new List<string>();

        /// <summary>
        /// 初始化格式字符串
        /// </summary>
        public static void InitDateTimeFormat()
        {
            DateTimeConvert.DateTimeFormatList.Clear();

            string[] dateTimeFormats = new string[]
            {
                "yyyy-MM-dd",
                "yyyy/MM/dd",
                "yyyy-MM-d",
                "yyyy-M-dd",
                "yyyy-M-d",
                "yyyyMMdd",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy/MM/dd HH:mm:ss",
                "yyyy-MM-ddHH:mm:ss",
                "yyyy/MM/ddHH:mm:ss",
                "yyyy-MM-dd HH:mm:ss.fff",
                "yyyy/MM/dd HH:mm:ss.fff",
                "yyyy-MM-ddHH:mm:ss.fff",
                "yyyy/MM/ddHH:mm:ss.fff",
            };

            DateTimeConvert.DateTimeFormatList.AddRange(dateTimeFormats);
        }

        public static bool TryPause(string s, out DateTime result)
        {
            if (DateTimeConvert.DateTimeFormatList.Count == 0)
                DateTimeConvert.InitDateTimeFormat();

            if (DateTime.TryParse(s, out result))
                return true;

            foreach (var format in DateTimeConvert.DateTimeFormatList)
            {
                if (DateTime.TryParseExact(s, format, null, DateTimeStyles.None, out result))
                    return true;
            }

            result = DateTime.MinValue;
            return false;
        }
    }
}
