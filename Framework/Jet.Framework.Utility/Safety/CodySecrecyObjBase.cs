using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class CodySecrecyObjBase
    {
        public int CustomerKey { get; set; }
        public string CustomerName { get; set; }

        public virtual string GetInfo()
        {
            return string.Format("客户Id:{0}\r\n客户:{1}",
                                this.CustomerKey,
                                this.CustomerName);
        }

        public static void LoadBytes(SmartObjectSerializer serializer, CodySecrecyObjBase obj)
        {
            serializer.Write(obj.CustomerKey);
            serializer.Write(obj.CustomerName);
        }

        public static void LoadObj(SmartObjectDeserializer deserializer, CodySecrecyObjBase obj)
        {
            obj.CustomerKey = deserializer.ReadInt32();
            obj.CustomerName = deserializer.ReadString();
        }
    }
}
