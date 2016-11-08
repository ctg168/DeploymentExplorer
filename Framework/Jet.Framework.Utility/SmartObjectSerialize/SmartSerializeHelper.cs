using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class SmartSerializeHelper
    {
        public static byte[] SerializeObject<T>(T obj, Action<SmartObjectSerializer, T> writeFunc, bool isCompress = true)
            where T : new()
        {
            SmartObjectSerializer serializer = new SmartObjectSerializer(isCompress);
            serializer.WriteObject(obj, writeFunc);
            return serializer.GetBinary();
        }

        public static byte[] SerializeObjectList<T>(IList<T> objList, Action<SmartObjectSerializer, T> writeFunc, bool isCompress = true)
            where T : new()
        {
            SmartObjectSerializer serializer = new SmartObjectSerializer(isCompress);
            serializer.WriteObjectList(objList, writeFunc);
            return serializer.GetBinary();
        }

        public static T DeserializeObject<T>(byte[] objBytes, Action<SmartObjectDeserializer, T> readFunc)
           where T : new()
        {
            SmartObjectDeserializer deserializer = new SmartObjectDeserializer(objBytes);
            return deserializer.ReadObject(readFunc);
        }

        public static List<T> DeserializeObjectList<T>(byte[] objBytes, Action<SmartObjectDeserializer, T> readFunc)
            where T : new()
        {
            SmartObjectDeserializer deserializer = new SmartObjectDeserializer(objBytes);
            return deserializer.ReadObjectList(readFunc);
        }
    }
}
