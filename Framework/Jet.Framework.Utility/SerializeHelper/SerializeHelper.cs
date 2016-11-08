using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Jet.Framework.Utility
{
    public static partial class SerializeHelper
    {
        public static byte[] ObjectToDataContractByte(object obj, Type type)
        {
            if (obj == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer(type);
                ser.WriteObject(ms, obj);
                return ms.ToArray();
            }
        }

        public static byte[] ObjectToDataContractByte<T>(T obj)
        {
            return ObjectToDataContractByte(obj, typeof(T));
        }
        
        public static object DataContractByteToObject(byte[] datas, Type type)
        {
            if (datas == null)
                return null;

            using (MemoryStream ms = new MemoryStream(datas))
            {
                DataContractSerializer ser = new DataContractSerializer(type);
                return ser.ReadObject(ms);
            }
        }

        public static T DataContractByteToObject<T>(byte[] datas)
        {
            return (T)DataContractByteToObject(datas, typeof(T));
        }        
    }
}
