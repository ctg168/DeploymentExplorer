using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

namespace Jet.Framework.Utility
{
    public static class IDictionaryExtend
    {
        public static T GetKeyValue<T>(this IDictionary _This, object key, T defaultValue = default(T))
        {
            if (_This.Contains(key))
                return (T)_This[key];
            else
                return defaultValue;
        }

        /// <summary>
        /// 根据条件删除所有满足的项
        /// </summary>
        public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<KeyValuePair<TKey, TValue>, bool> checkFun)
        {
            List<TKey> removeKeyList = new List<TKey>();
            foreach (var keyValuePair in dic)
            {
                if (checkFun(keyValuePair))
                {
                    removeKeyList.Add(keyValuePair.Key);
                }
            }
            foreach (var key in removeKeyList)
            {
                dic.Remove(key);
            }
        }
    }
}
