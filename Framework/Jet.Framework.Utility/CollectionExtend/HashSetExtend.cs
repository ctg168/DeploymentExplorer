using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class HashSetExtend
    {
        public static void AddRange<T>(this HashSet<T> hs, IEnumerable<T> list)
        {
            foreach (var obj in list)
            {
                if (!hs.Contains(obj))
                    hs.Add(obj);
            }
        }
    }
}
