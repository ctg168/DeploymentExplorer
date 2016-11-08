using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static partial class StringExtend
    {
        private static readonly List<string> ChineseNOList = new List<string>(new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十", "二十一", "二十二", "二十三", "二十四", "二十五", "二十六", "二十七", "二十八", "二十九", "三十" });

        /// <summary>
        /// 得到中文大题号
        /// <remarks>        
        public static string GetChineseNo(this int num)
        {
            if (num > 0 && num <= StringExtend.ChineseNOList.Count)
            {
                return StringExtend.ChineseNOList[num - 1];
            }
            else
            {
                return num.ToString();
            }
        }

        public static int GetEngNo(this string num)
        {
            int index = StringExtend.ChineseNOList.IndexOf(num);
            return index + 1;
        }
    }
}
