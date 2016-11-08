using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Jet.Framework.Utility
{
    public static class IListExtend
    {
        /// <summary>
        /// 倒序循环删除指定条件的子项
        /// </summary>
        public static void Remove<T>(this IList<T> list, Func<T, bool> checkFun)
        {
            for (int i = list.Count - 1; i > -1; i--)
            {
                if (checkFun(list[i]))
                    list.RemoveAt(i);
            }
        }

        /// <summary>
        /// 根据指定的键和值来构建字典
        /// </summary>
        public static Dictionary<TKey, List<TVALUE>> BuildGroupDictionary<TKey, TVALUE>(this ICollection<TVALUE> list, Func<TVALUE, TKey> getKeyFun)
        {
            return BuildGroupDictionary(list, getKeyFun, r => r);
        }

        /// <summary>
        /// 根据指定的键和值来构建字典
        /// </summary>
        public static Dictionary<TKey, List<TVALUE>> BuildGroupDictionary<TKey, TVALUE, T>(this ICollection<T> list, Func<T, TKey> getKeyFun, Func<T, TVALUE> getValueFun)
        {
            Dictionary<TKey, List<TVALUE>> dic = new Dictionary<TKey, List<TVALUE>>();
            foreach (T obj in list)
            {
                TKey key = getKeyFun(obj);
                TVALUE value = getValueFun(obj);

                List<TVALUE> listTemp = null;
                if (dic.ContainsKey(key))
                {
                    listTemp = dic[key];
                }
                else
                {
                    listTemp = new List<TVALUE>();
                    dic.Add(key, listTemp);
                }
                listTemp.Add(value);
            }
            return dic;
        }

        /// <summary>
        /// 根据指定的键和值来构建字典
        /// </summary>
        public static Dictionary<TKey1, Dictionary<TKey2, TValue>> BuildDictionaryGroupDictionary<TKey1, TKey2, TValue>(this ICollection<TValue> list, Func<TValue, TKey1> getKeyFun1, Func<TValue, TKey2> getKeyFun2)
        {
            return BuildDictionaryGroupDictionary(list, getKeyFun1, getKeyFun2, r => r);
        }

        /// <summary>
        /// 根据指定的键和值来构建字典
        /// </summary>
        public static Dictionary<TKey1, Dictionary<TKey2, TValue>> BuildDictionaryGroupDictionary<TKey1, TKey2, TValue, T>(this ICollection<T> list, Func<T, TKey1> getKeyFun1, Func<T, TKey2> getKeyFun2, Func<T, TValue> getValueFun)
        {
            var dic1 = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
            foreach (T obj in list)
            {
                TKey1 key1 = getKeyFun1(obj);
                TKey2 key2 = getKeyFun2(obj);
                TValue value = getValueFun(obj);

                Dictionary<TKey2, TValue> dic2 = null;
                if (dic1.ContainsKey(key1))
                {
                    dic2 = dic1[key1];
                }
                else
                {
                    dic2 = new Dictionary<TKey2, TValue>();
                    dic1.Add(key1, dic2);
                }
                dic2.Add(key2, value);
            }
            return dic1;
        }



        /// <summary>
        /// 根据前缀在一组序列内生成新的名称
        /// </summary>
        public static string GenNewName(this IList<string> list, string prefix)
        {
            if (null == list) { return prefix + "1"; }

            string newName = string.Empty;
            for (int i = 1; i <= list.Count + 1; i++)
            {
                newName = string.Format("{0}{1}", prefix, i);
                if (!list.Contains(newName))
                {
                    break;
                }
            }
            return newName;
        }    

        /// <summary>
        /// 将List转换为以逗号分割的字符串
        /// </summary>
        public static string ListToStringAppendComma<T>(this IEnumerable<T> list, string splitStr = ",")
        {
            if (list.Count() == 0) return string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (var item in list)
            {
                builder.Append(item.ToString() + splitStr);
            }

            builder.Remove(builder.Length - splitStr.Length, splitStr.Length);
            return builder.ToString();
        }


        /// <summary>
        /// Try the string to int list by comma.
        /// </summary>
        public static bool TryStringToIntListByComma(this string strList, out List<int> list)
        {
            list = new List<int>();
            if (string.IsNullOrEmpty(strList)) return false;
            if (strList.IndexOf(",") != -1)
            {
                string[] strArray = strList.Split(',');

                foreach (var item in strArray)
                {
                    int i = 0;
                    if (int.TryParse(item, out i))
                    {
                        list.Add(i);
                    }
                }
                return true;
            }
            else
            {
                int i = 0;
                if (int.TryParse(strList, out i))
                {
                    list.Add(i);
                }
            }
            return false;
        }

        /// <summary>
        /// Strings to int list string by comma.
        /// </summary>
        public static List<string> StringToIntListStringByComma(this string strList)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(strList)) return list;
            if (strList.IndexOf(",") != -1)
            {
                string[] strArray = strList.Split(',');

                foreach (var item in strArray)
                {
                    list.Add(item);
                }
            }
            else
            {
                list.Add(strList);
            }
            return list;
        }

        public static List<T> ToBaseList<T>(this IEnumerable list)
        {
            List<T> parentList = new List<T>();

            foreach (var obj in list)
            {
                parentList.Add((T)obj);
            }

            return parentList;
        }

        public static List<T> Copy<T>(this List<T> list)
        {
            List<T> listTemp = new List<T>();
            listTemp.AddRange(list.ToArray());
            return listTemp;
        }

        /// <summary>
        /// 根据指定数量分组
        /// </summary>
        public static Dictionary<int, List<T>> GetGroupDic<T>(this IList<T> list, int skipCount = 300)
        {
            Dictionary<int, List<T>> timesdict = new Dictionary<int, List<T>>();
            if (list.Count > 0)
            {
                int times = list.Count;
                int allcount = 0;
                for (int i = 0; i < times; i++)
                {
                    List<T> empList = list.Skip(allcount).Take(skipCount).ToList();
                    if (allcount >= times)
                    {
                        break;
                    }
                    else
                    {
                        timesdict.Add(i, empList);
                    }
                    allcount += empList.Count();
                }
            }

            return timesdict;
        }

        /// <summary>
        /// 按指定顺序计算排名
        /// </summary>
        public static void ComputePaiMing<T>(this List<T> list, Comparison<T> comparison, Action<T, int> setPaiMingAction, bool isSort = false)
        {
            List<T> listTemp;
            if (isSort)
                listTemp = list;
            else
                listTemp = new List<T>(list.ToArray());

            listTemp.Sort(comparison);
            T preObj = default(T);
            int paiMing = 1;
            for (int i = 0; i < listTemp.Count; i++)
            {
                T obj = listTemp[i];
                if (preObj != null)
                {
                    if (comparison(preObj, obj) != 0)//比较相同就排名相同
                        paiMing = i + 1;
                }

                setPaiMingAction(obj, paiMing);
                preObj = obj;
            }
        }

        /// <summary>
        /// 递归遍历一个树结构对象
        /// </summary>
        public static void ForeachTree<T>(this IList<T> list, Func<T, IList<T>> GetObjChildren, Action<T> objAction)
        {
            foreach (var obj in list)
            {
                var children = GetObjChildren(obj);

                objAction(obj);
                ForeachTree(children, GetObjChildren, objAction);
            }
        }

        private static void ForeachTree<T>(T obj, Func<T, IList<T>> GetObjChildren, Action<T, T> objAction)
        {
            var children = GetObjChildren(obj);

            foreach (var child in children)
            {
                objAction(obj, child);
                ForeachTree(child, GetObjChildren, objAction);
            }
        }

        /// <summary>
        /// 递归遍历一个树结构对象
        /// </summary>
        public static void ForeachTree<T>(this IList<T> list, Func<T, IList<T>> GetObjChildren, Action<T, T> objAction)
        {
            foreach (var obj in list)
            {
                objAction(default(T), obj);
                ForeachTree(obj, GetObjChildren, objAction);
            }
        }

        public static bool IsSameList<T>(this IList<T> list1, IList<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false;
            }

            return true;
        }
    }
}
