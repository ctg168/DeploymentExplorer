using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class CodyPQRockeyArm : CodySecrecyObjBase
    {
        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime? EmpowerDate { get; set; }

        public static void LoadBytes(SmartObjectSerializer serializer, CodyPQRockeyArm obj)
        {
            CodySecrecyObjBase.LoadBytes(serializer, obj);

            serializer.Write(obj.EmpowerDate);
        }

        public static void LoadObj(SmartObjectDeserializer deserializer, CodyPQRockeyArm obj)
        {
            CodySecrecyObjBase.LoadObj(deserializer, obj);

            obj.EmpowerDate = deserializer.ReadNullableDateTime();
        }

        public override string GetInfo()
        {
            return string.Format("客户Id:{0}\r\n客户:{1}\r\n过期时间:{2}",
                                this.CustomerKey,
                                this.CustomerName,
                                this.EmpowerDate.HasValue ? this.EmpowerDate.Value.ToLongDateString() : "无限期");
        }
    }
}
