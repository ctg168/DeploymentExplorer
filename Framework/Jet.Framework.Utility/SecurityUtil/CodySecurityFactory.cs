using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class CodySecurityFactory
    {
        /// <summary>
        /// 根据版本号创建加(解)密器
        /// </summary>
        public static CodySecurityBase CreateCodySecurity(byte version)
        {
            if (version == CodySecurityHelper.VersionNO1)
            {
                return new CodySecurity_1();
            }

            throw new Exception("无法获得相应的加密器");
        }
    }
}
