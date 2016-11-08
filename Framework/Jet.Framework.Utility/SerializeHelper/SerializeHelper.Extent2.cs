using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Jet.Framework.Utility
{
    public partial class SerializeHelper
    {
        public static string ObjectToDataContractString(object obj, Type type)
        {
            if (obj == null)
                return string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter tw = new XmlTextWriter(sw);

                DataContractSerializer ser = new DataContractSerializer(type);
                ser.WriteObject(tw, obj);
                return sw.ToString();
            }
        }

        public static string ObjectToDataContractString<T>(T obj)
        {
            return ObjectToDataContractString(obj, typeof(T));
        }

        public static object DataContractStringToObject(string datas, Type type)
        {
            if (string.IsNullOrEmpty(datas))
                return null;

            using (StringReader sr = new StringReader(datas))
            {
                DataContractSerializer ser = new DataContractSerializer(type);
                XmlTextReader tr = new XmlTextReader(sr);
                return ser.ReadObject(tr);
            }
        }

        public static T DataContractStringToObject<T>(string datas)
        {
            return (T)DataContractStringToObject(datas, typeof(T));
        }
    }
}
