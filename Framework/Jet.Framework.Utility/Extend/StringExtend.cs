using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jet.Framework.Utility
{
    public static partial class StringExtend
    {
        public static string StrEncode(this string str)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                builder.Append((char)(str[i] + 10 - 1 * 2));
            }
            return builder.ToString();
        }


        public static string StrDecode(this string str)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                builder.Append((char)(str[i] - 10 + 1 * 2));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 分割字符串列表
        /// eg:{A/B,A/B/C} => {A,A/B,A/B/C}
        /// </summary>
        /// <param name="sourceStringList"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static List<string> GetdistinctStringList(List<string> sourceStringList, char splitChar)
        {
            List<string> result = new List<string>();
            sourceStringList.ForEach(s =>
            {
                result.AddRange(s.StringSplit(splitChar).Keys);
            });
            return result.Distinct().ToList();
        }

        /// <summary>
        /// 分割制定的字符串
        /// </summary>
        /// <param name="strlist"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static Dictionary<string, string> StringListSplit(this IList<string> strlist, char splitChar)
        {
            Dictionary<string, string> tDict = new Dictionary<string, string>();
            //var query =from s in strlist
            var orderlist = strlist.Distinct().OrderByDescending(r => r);

            foreach (var item in orderlist)
            {
                if (!tDict.ContainsKey(item))
                {
                    string str = item;
                    var dic = StringSplit(str, splitChar);
                    foreach (var key in dic.Keys)
                    {
                        if (!tDict.ContainsKey(key))
                        {
                            tDict.Add(key, dic[key]);
                        }
                    }
                }
            }

            return tDict;
        }

        /// <summary>
        /// 分割指定的字符串 
        /// </summary>
        /// <param name="strlist"></param>
        /// <param name="splitChar"></param>
        /// <returns>带深度的字典</returns>
        public static Dictionary<int, Dictionary<string, string>> ListSplitToDepthDict(this IList<string> strlist, char splitChar)
        {
            Dictionary<string, string> tDict = new Dictionary<string, string>();

            //var query =from s in strlist
            var orderlist = strlist.Distinct().OrderByDescending(r => r);

            Dictionary<int, Dictionary<string, string>> alldic = new Dictionary<int, Dictionary<string, string>>();

            foreach (var item in orderlist)
            {
                if (!tDict.ContainsKey(item))
                {
                    string str = item;
                    var dic = StringSplit(str, splitChar);
                    foreach (var key in dic.Keys)
                    {
                        if (!tDict.ContainsKey(key))
                        {
                            int depth = key.Split(splitChar).Length;
                            tDict.Add(key, dic[key]);
                            if (!alldic.ContainsKey(depth))
                            {
                                alldic.Add(depth, new Dictionary<string, string>());
                            }
                            alldic[depth].Add(key, dic[key]);
                        }
                    }
                }
            }

            return alldic;
        }

        /// <summary>
        /// 分割制定的字符串 示例:/aa/bb/cc/,返回字典以"aa","aa/bb","aa/bb/cc"为Key,Value分别对应为"aa","bb","cc"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static Dictionary<string, string> StringSplit(this string str, char splitChar)
        {
            if (str.StartsWith(splitChar.ToString())) str = str.Remove(0, 1);
            if (str.EndsWith(splitChar.ToString())) str = str.Remove(str.Length - 1, 1);

            Dictionary<string, string> tDict = new Dictionary<string, string>();
            string[] aa = str.Split(splitChar);
            int j = 0;
            int i;
            for (i = 0; i < str.Length; i++)
            {
                if (str[i] == splitChar)
                {
                    tDict.Add(str.Substring(0, i), aa[j++]);
                }
            }
            tDict.Add(str, str.Substring(str.LastIndexOf(splitChar) + 1));
            return tDict;
        }

        /// <summary>
        /// 去多余空格
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string TrimAll(this string inputString)
        {
            inputString = inputString.Trim().Replace("\n", "").Replace(" ", "").Replace("　", "");
            return inputString;
        }

        /// <summary>
        /// 切割指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string StrSplit(this string str, char splitChar)
        {
            return str.TrimStart(splitChar).TrimEnd(splitChar);
        }

        /// <summary>
        /// 判断字符是否英文半角字符或标点
        /// </summary>
        /// <remarks>
        /// 32    空格
        /// 33-47    标点
        /// 48-57    0~9
        /// 58-64    标点
        /// 65-90    A~Z
        /// 91-96    标点
        /// 97-122    a~z
        /// 123-126  标点
        /// </remarks>
        public static bool IsBjChar(this char c)
        {
            int i = (int)c;
            return i >= 32 && i <= 126;
        }

        /// <summary>
        /// 判断字符是否全角字符或标点
        /// </summary>
        /// <remarks>
        /// <para>全角字符 - 65248 = 半角字符</para>
        /// <para>全角空格例外</para>
        /// </remarks>
        public static bool IsQjChar(this char c)
        {
            if (c == '\u3000') //全角空格
                return true;

            int i = (int)c - 65248;
            if (i < 32) return false;
            return IsBjChar((char)i);
        }

        /// <summary>
        /// 将字符串中的全角字符转换为半角
        /// </summary>
        public static string ToBj(this string s)
        {
            if (s == null || s.Trim() == string.Empty)
                return s;

            StringBuilder sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\u3000')//全角空格
                    sb.Append('\u0020');//半角空格
                else if (IsQjChar(s[i]))
                    sb.Append((char)((int)s[i] - 65248));
                else
                    sb.Append(s[i]);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)//半角空格
                {
                    c[i] = (char)12288;//全角空格
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)//全角空格
                {
                    c[i] = (char)32;//半角空格
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 获得最后路径
        /// </summary>
        /// <param name="path">路径字符串</param>
        /// <param name="split">分割符</param>
        /// <param name="length">获得级数</param>
        /// <returns></returns>
        public static string PathIntercept(string path, char split, int length)
        {
            return GetLastSubPath(path, length, split).Replace(split, ' ');
        }

        /// <summary>
        /// 获得字符串最后面的参数length段子路径
        /// </summary>
        /// <param name="path">路径字符串</param>
        /// <param name="split">分割符</param>
        /// <param name="length">获得级数</param>
        /// <returns></returns>
        public static string GetLastSubPath(string path, int length, char split = '/')
        {
            if (length <= 0)
            {
                throw "length参数错误".CreateException();
            }
            if (string.IsNullOrEmpty(path)) return string.Empty;
            int count = path.Split(split).Length;

            if (count > length)
            {
                int index = path.Length;

                for (int i = 0; i < length; i++)
                {
                    index = path.LastIndexOf(split, index - 1);
                }

                index++;

                return path.Substring(index, (path.Length - index));
            }

            return path;
        }

        /// <summary>
        /// 获得learningOnline文件夹路径格式的字符串 eg:从 AAA/BBB/CCC/DDD 得到 CCC DDD
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLearningOnlinePathFormatString_Folder(this string path)
        {
            //return GetLastSubPath(GetSubPath(path), 2).Replace('/',' ');
            return GetLastSubPath(path, 2).Replace('/', ' ');
        }

        /// <summary>
        /// 获得learningOnline部门路径格式的字符串 \
        /// eg:从AAA/BBB/CCC/DDD 得到 BBB CCC  
        /// eg:从AAA/BBB 得到 BBB
        /// eg:从AAA 得到 AAA
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string GetLearningOnlinePathFormatString_Dept(this string path, int length = 2, char split = '/')
        {
            if (length <= 0)
            {
                throw "length参数错误".CreateException();
            }
            if (string.IsNullOrEmpty(path)) return string.Empty;
            var strList = path.Split(split);

            if (1 >= strList.Length)
            {
                return path;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 1; i < strList.Count(); i++)
                {
                    stringBuilder.AppendFormat("{0} ", strList[i]);
                }
                return stringBuilder.ToString();
            }
        }

        private const char DeptFullPathSplitChar = '/';

        /// <summary>
        /// 获得FullPath的路径： 
        ///   例如：科迪公司/内容提供部/一班/一组 ==> 内容提供部/一班/一组
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetSubPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            int Levels = path.Split(DeptFullPathSplitChar).Count();
            if (Levels <= 1) return path;

            int firstIndex = path.IndexOf(DeptFullPathSplitChar) + 1;

            return path.Substring(firstIndex);
        }

        /// <summary>
        /// 获得FullPath的路径： 
        ///   例如：科迪公司/内容提供部/一班/一组 ==> 科迪公司/内容提供部/一班
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetParentPath(string path, char splitChar = '/')
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            int splitCharIndex = path.LastIndexOf(splitChar);

            if (splitCharIndex == -1)
            {
                return string.Empty;
            }

            return path.Substring(0, splitCharIndex);
        }

        /// <summary>
        /// 获得路径的最后一级
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLastPath(string path)
        {
            char splitChar = '/';
            if (string.IsNullOrEmpty(path)) return string.Empty;
            return path.Split(splitChar).Last();

        }
        /// <summary>
        /// 获得路径的最后一级,超出指定长度部分不显示...
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLastPath(string path, int subLength)
        {
            char splitChar = '/';
            if (string.IsNullOrEmpty(path)) return string.Empty;
            string str = path.Split(splitChar).Last();
            if (str.Length > subLength)
            {
                str = str.Substring(0, subLength) + "...";
            }

            return str;//path.Split(splitChar).Last();

        }

        /// <summary>
        /// 去掉空白字符
        /// </summary>
        public static string RemoveWhiteSpace(this string str)
        {
            return Regex.Replace(str, "\\s", "");
        }

        public static int ParseToInt(this string str)
        {
            str = str.Replace('０', '0').Replace('１', '1').Replace('２', '2').Replace('３', '3').Replace('４', '4').Replace('５', '5').Replace('６', '6').Replace('７', '7').Replace('８', '8').Replace('９', '9');
            return int.Parse(str);
        }

        public static decimal ParseToDecimal(this string str)
        {
            str = str.Replace('０', '0').Replace('１', '1').Replace('２', '2').Replace('３', '3').Replace('４', '4').Replace('５', '5').Replace('６', '6').Replace('７', '7').Replace('８', '8').Replace('９', '9');
            return decimal.Parse(str);
        }

        private static readonly string RegexStrs = @"\/|*.?+$^[](){}";//注意\必须是第一个字符
        public static string ToRegexString(this string str)
        {
            foreach (var regexChar in RegexStrs)
            {
                str = str.Replace(regexChar.ToString(), "\\" + regexChar);
            }

            return str;
        }
    }
}
