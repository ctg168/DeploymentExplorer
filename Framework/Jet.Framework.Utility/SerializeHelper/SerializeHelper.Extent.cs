using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Jet.Framework.Utility
{
    public static partial class SerializeHelper
    {
        public static string ObjectToBase64String(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memStream, obj);
                byte[] data = memStream.ToArray();
                return Convert.ToBase64String(data);
            }
        }

        public static object Base64StringToObject(string objstr)
        {
            if (objstr == null)
            {
                return null;
            }

            byte[] data = Convert.FromBase64String(objstr);

            using (MemoryStream memStream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(memStream);
                return obj;
            }
        }

        public static string ObjectToXmlString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer packageSerializer = new XmlSerializer(obj.GetType());
                packageSerializer.Serialize(sw, obj);
                string objxml = sw.ToString();
                return objxml;
            }
        }


        public static string ObjectToXmlString(object obj, Encoding encoding)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            string objXml = null;

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer packageSerializer = new XmlSerializer(obj.GetType());

                using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { Encoding = encoding }))
                {
                    packageSerializer.Serialize(writer, obj);
                    objXml = encoding.GetString(stream.ToArray());
                }
            }
            return objXml;
        }

        public static object XmlStringToObject(string objxml, Type type)
        {
            if (objxml == null || objxml.Length == 0)
            {
                return null;
            }

            using (StringReader sr = new StringReader(objxml))
            {
                XmlSerializer packageSerializer = new XmlSerializer(type);
                object obj = packageSerializer.Deserialize(sr);
                return obj;
            }
        }

        public static T XmlStringToObject<T>(string objxml, Type type)
        {
            return (T)XmlStringToObject(objxml, type);
        }

        public static byte[] ObjectToByte(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memStream, obj);
                byte[] data = memStream.ToArray();
                return data;
            }
        }

        public static object ByteToObject(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            using (MemoryStream memStream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(memStream);
                return obj;
            }
        }

        public static object ByteToObject(byte[] data, int index, int count)
        {
            if (data == null)
            {
                return null;
            }

            using (MemoryStream memStream = new MemoryStream(data, index, count))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(memStream);
                return obj;
            }
        }


        public static bool ByteEquals(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return false;
            return true;
        }

        /// <summary>
        /// 深度克隆一个对象
        /// </summary>
        public static T DeepClone<T>(T obj)
        {
            byte[] bytes = ObjectToByte(obj);
            T cloneObj = (T)ByteToObject(bytes);
            return cloneObj;
        }
    }
}
