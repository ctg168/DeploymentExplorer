using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Jet.Framework.Utility
{
    public static class ObservableCollectionExtend
    {
        /// <summary>
        /// 根据条件删除所有满足的项
        /// </summary>
        public static ObservableCollection<T> RemoveObjList<T>(this ObservableCollection<T> list, IList<T> removeObjList)
        {
            List<T> newObjList = new List<T>();
            HashSet<T> removeHs = new HashSet<T>(removeObjList);

            foreach (var obj in list)
            {
                if (!removeHs.Contains(obj))
                    newObjList.Add(obj);
            }

            return new ObservableCollection<T>(newObjList);
        }
    }
}
