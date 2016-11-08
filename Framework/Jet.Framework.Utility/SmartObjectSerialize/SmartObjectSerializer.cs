using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class SmartObjectSerializer : IDisposable
    {
        public SmartObjectSerializer(bool isCompress = true)
        {
            this.IsCompress = isCompress;
            this.ContextStream = new MemoryStream();
            this.ObjectDict = new Dictionary<object, int>();

            this.Write(this.IsCompress);
        }

        public void Dispose()
        {
            this.ContextStream.Dispose();
        }

        private bool IsCompress;//是否压缩
        private int IdSeed = 0;

        private Dictionary<object, int> ObjectDict;

        private MemoryStream ContextStream;

        public byte[] GetBinary()
        {
            if (this.IsCompress)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(BitConverter.GetBytes(this.IsCompress), 0, sizeof(bool));
                    byte[] compressBytes = SharpZipHelper.Compress(this.ContextStream.ToArray(), sizeof(bool));
                    ms.Write(compressBytes, 0, compressBytes.Length);
                    return ms.ToArray();
                }
            }
            else
            {
                this.Write(this.IsCompress);
                return this.ContextStream.ToArray();
            }

        }

        public void WriteObject<T>(T obj, Action<SmartObjectSerializer, T> writeFunc)
            where T : new()
        {
            if (obj != null)
            {
                if (this.ObjectDict.ContainsKey(obj))
                {
                    int id = this.ObjectDict[obj];
                    this.Write(id);
                }
                else
                {
                    int id = ++IdSeed;
                    this.ObjectDict.Add(obj, id);
                    this.Write(id);
                    writeFunc(this, obj);
                }
            }
            else
                this.Write(0);
        }

        public void WriteObjectList<T>(IList<T> objList, Action<SmartObjectSerializer, T> writeFunc)
            where T : new()
        {
            this.Write(objList.Count);
            foreach (var obj in objList)
            {
                this.WriteObject(obj, writeFunc);
            }
        }

        public void WriteValueList<T>(IList<T> valList, Action<SmartObjectSerializer, T> writeFunc)    
        {
            this.Write(valList.Count);
            foreach (var val in valList)
            {
                writeFunc(this, val);
            }
        }

        public void WriteObjectDic<TKey, TValue>(IDictionary<TKey, TValue> objDic, Action<SmartObjectSerializer, TKey> writeKeyFunc, Action<SmartObjectSerializer, TValue> writeValueFunc)
        {
            this.Write(objDic.Count);
            foreach (var item in objDic)
            {
                writeKeyFunc(this, item.Key);
                writeValueFunc(this, item.Value);
            }
        }

        #region 引用类型
        public void Write(byte[] val)
        {
            this.Write(val != null);

            if (val != null)
            {
                //写入字节数组长度
                this.Write(val.Length);

                //写入字节数组内容
                this.ContextStream.Write(val, 0, val.Length);
            }
        }

        public void Write(string val)
        {
            this.Write(val != null);

            if (val != null)
            {
                byte[] bytesTemp = Encoding.Unicode.GetBytes(val);
                this.Write(bytesTemp.Length);
                this.ContextStream.Write(bytesTemp, 0, bytesTemp.Length);
            }
        }
        #endregion

        #region 值类型
        public void Write(bool val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(short val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(int val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(Int64 val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(float val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(double val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }

        public void Write(decimal val)
        {
            this.Write(val.ToString());
        }

        public void Write(DateTime val)
        {
            long ticks = (val - DateTime.MinValue).Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            this.ContextStream.Write(bytes, 0, bytes.Length);
        }
        #endregion

        #region 可空值类型
        public void Write(bool? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                byte[] bytes = BitConverter.GetBytes(val.Value);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(short? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                byte[] bytes = BitConverter.GetBytes(val.Value);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(int? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                byte[] bytes = BitConverter.GetBytes(val.Value);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(Int64? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                byte[] bytes = BitConverter.GetBytes(val.Value);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(float? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                byte[] bytes = BitConverter.GetBytes(val.Value);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(double? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                byte[] bytes = BitConverter.GetBytes(val.Value);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Write(decimal? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                this.Write(val.Value.ToString());
            }
        }

        public void Write(DateTime? val)
        {
            this.Write(val.HasValue);

            if (val.HasValue)
            {
                long ticks = (val.Value - DateTime.MinValue).Ticks;
                byte[] bytes = BitConverter.GetBytes(ticks);
                this.ContextStream.Write(bytes, 0, bytes.Length);
            }
        }
        #endregion

        #region Object
        public void Write(Type t, object val)
        {
            if (t.IsEnum)
            {
                this.Write((int)val);
                return;
            }

            if (t == typeof(byte[]))
            {
                this.Write((byte[])val);
            }
            else if (t == typeof(string))
            {
                this.Write((string)val);
            }
            else if (t == typeof(bool))
            {
                this.Write((bool)val);
            }
            else if (t == typeof(bool?))
            {
                this.Write((bool?)val);
            }
            else if (t == typeof(short))
            {
                this.Write((short)val);
            }
            else if (t == typeof(short?))
            {
                this.Write((short?)val);
            }
            else if (t == typeof(int))
            {
                this.Write((int)val);
            }
            else if (t == typeof(int?))
            {
                this.Write((int?)val);
            }
            else if (t == typeof(Int64))
            {
                this.Write((Int64)val);
            }
            else if (t == typeof(Int64?))
            {
                this.Write((Int64?)val);
            }
            else if (t == typeof(float))
            {
                this.Write((float)val);
            }
            else if (t == typeof(float?))
            {
                this.Write((float?)val);
            }
            else if (t == typeof(double))
            {
                this.Write((double)val);
            }
            else if (t == typeof(double?))
            {
                this.Write((double?)val);
            }
            else if (t == typeof(decimal))
            {
                this.Write((decimal)val);
            }
            else if (t == typeof(decimal?))
            {
                this.Write((decimal?)val);
            }
            else if (t == typeof(DateTime))
            {
                this.Write((DateTime)val);
            }
            else if (t == typeof(DateTime?))
            {
                this.Write((DateTime?)val);
            }
            else
            {
                throw new Exception(string.Format("未识别的序列化类型[{0}]", t.ToString()));
            }
        }
        #endregion
    }
}
