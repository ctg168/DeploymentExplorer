using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class ArrayHelper
    {
        /// <summary>
        /// 判断两个字节数组是否相同
        /// </summary>
        public static bool IsSameByteArray(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1 == null && bytes2 == null)
            {
                return true;
            }
            else if (bytes1 != null && bytes2 != null)
            {
                if (bytes1.Length != bytes2.Length)
                    return false;

                for (int i = 0; i < bytes1.Length; i++)
                {
                    if (bytes1[i] != bytes2[i])
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
