using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class ObjectCloneExtend
    {
        public static List<T> Clone<T>(this List<T> objList)
            where T : IObjectClone<T>
        {
            List<T> cloneObjList = new List<T>();
            objList.ForEach(obj =>
            {
                cloneObjList.Add(obj.Clone());
            });
            return cloneObjList;
        }
    }
}
