using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !SILVERLIGHT
using System.Web;
#endif

namespace Jet.Framework.Utility
{
    public class StringCompare : IComparer<string>
    {
        public static StringCompare Instance = new StringCompare();
        private StringCompare()
        {
            this.StringCompareDic.Add('一', 1);
            this.StringCompareDic.Add('二', 2);
            this.StringCompareDic.Add('三', 3);
            this.StringCompareDic.Add('四', 4);
            this.StringCompareDic.Add('五', 5);
            this.StringCompareDic.Add('六', 6);
            this.StringCompareDic.Add('七', 7);
            this.StringCompareDic.Add('八', 8);
            this.StringCompareDic.Add('九', 9);
            this.StringCompareDic.Add('十', 10);
        }

        private Dictionary<char, int> StringCompareDic = new Dictionary<char, int>();

        public int Compare(string str1, string str2)
        {
            int minLength = Math.Min(str1.Length, str2.Length);

            for (int i = 0; i < minLength; i++)
            {
                char c1 = str1[i];
                char c2 = str2[i];

                if (c1 != c2)
                {
                    if (StringCompareDic.ContainsKey(c1) && StringCompareDic.ContainsKey(c2))
                        return StringCompareDic[c1] - StringCompareDic[c2];
                    else
                        return c1 - c2;
                }
            }

            return str1.Length - str2.Length;
        }
    }

    public static class StringHelper
    {

#if !SILVERLIGHT
        public static string EncodeDictionaryUri(Dictionary<string, string> dic)
        {
            List<string> list = new List<string>();
            foreach (string key in dic.Keys)
            {
                string val = dic[key];
                if (string.IsNullOrEmpty(val))
                    continue;

                list.Add(string.Format("{0}={1}", key,HttpContext.Current.Server.UrlEncode(val)));
            }

            return string.Join("&", list.ToArray());
        }
#endif

        /// 转全角的函数(SBC case)
        ///
        ///任意字符串
        ///全角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public static String ToSBC(String input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        ///
        /// 转半角的函数(DBC case)
        ///
        ///任意字符串
        ///半角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }


        public static HashSet<int> GetKeyWordIndexHs(string str, List<string> keyWordList)
        {
            HashSet<int> indexHs = new HashSet<int>();
            foreach (var keyWord in keyWordList)
            {
                int index = 0;
                while (true)
                {
                    index = str.IndexOf(keyWord, index);

                    if (index == -1)
                    {
                        break;
                    }
                    else
                    {
                        for (int i = 0; i < keyWord.Length; i++)
                        {
                            if (!indexHs.Contains(index + i))
                                indexHs.Add(index + i);
                        }

                        index++;
                    }
                }
            }
            return indexHs;
        }

        public static string Replace(this string text, List<string> oldStrList, char newChar)
        {
            var indexHs = StringHelper.GetKeyWordIndexHs(text, oldStrList);

            string newText = string.Empty;
            for (int i = 0; i < text.Length; i++)
            {
                if (indexHs.Contains(i))
                    newText += newChar;
                else
                    newText += text[i];
            }

            return newText;
        }
    }
}
