using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace Jet.Framework.Utility
{
    public static class ReflectionHelper
    {
        public static readonly MethodInfo MethodInfo_GroupBy_Source_KeySelector;

        static ReflectionHelper()
        {
            MethodInfo_GroupBy_Source_KeySelector = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(o => o.Name == "GroupBy" && o.GetGenericArguments().Length == 2 && o.GetParameters().Length == 2)
                    .Single();
        }

        public static IEnumerable GroupBy(Type t, Type k, object datalist, object keyselector)
        {
            return MethodInfo_GroupBy_Source_KeySelector.MakeGenericMethod(t, k).Invoke(null, new object[] { datalist, keyselector }) as IEnumerable;
        }
    }
}
