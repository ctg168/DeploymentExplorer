using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class RtfHelper
    {
        private static string StrInfoStart = "{\\info";
        private static string StrInfoEnd = "}\r\n";
        private static string StrColor = "{\\colortbl ;\\red0\\green0\\blue0;}\r\n";
        /// <summary>
        /// 去掉多余的信息
        /// </summary>
        public static string RemoveRtfInfo(string rtf)
        {
            int index1 = rtf.IndexOf(StrInfoStart);

            if (index1 > 0)
            {
                int index2 = rtf.IndexOf(StrInfoEnd, index1 + StrInfoStart.Length);

                if (index2 > 0)
                {
                    rtf = rtf.Remove(index1, index2 - index1 + StrInfoEnd.Length);
                }
            }

            return rtf.Replace(StrColor, string.Empty);
        }

        private static List<string> RtfKeyWordList = new List<string>(new string[] { @"\object", @"\pict", @"\trowd" });
        //判断一个Rtf是否只有文本信息
        public static bool IsRtfOnlyText(string rtf)
        {
            if (rtf == null) return false;

            foreach (string keyWord in RtfHelper.RtfKeyWordList)
            {
                if (rtf.Contains(keyWord))
                    return false;
            }

            return true;
        }

        //****************************************去掉结尾的回车**************************************{
        private const string EndRtf = "}\r\n";
        private const string EnterRtf = "\\par\r\n";
        public static bool IsEnter(string rtf, int index)
        {
            if (index < 0)
                return false;

            return rtf.Substring(index, EnterRtf.Length) == EnterRtf;
        }

        /// <summary>
        /// 去掉结尾的回车
        /// </summary>
        public static string RemoveEndEnter(string rtf)
        {
            rtf = rtf.TrimEnd('\0');

            if (rtf.EndsWith(EndRtf))
            {
                int index = rtf.Length - EndRtf.Length;
                int removeLength = 0;
                while (RtfHelper.IsEnter(rtf, index - EnterRtf.Length))
                {
                    index -= EnterRtf.Length;
                    removeLength += EnterRtf.Length;
                }
                if (removeLength > 0)
                    rtf = rtf.Remove(index, removeLength);
            }
            return rtf;
        }
        //****************************************去掉结尾的回车**************************************}
    }
}
