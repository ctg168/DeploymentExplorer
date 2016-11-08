using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public class SmartObjectDeserializer
    {
        public SmartObjectDeserializer(byte[] objBytes)
        {
            this.ObjBytes = objBytes;

            bool isCommpress = this.ReadBool();

            if (isCommpress)
            {
                this.ObjBytes = SharpZipHelper.Decompress(this.ObjBytes, sizeof(bool));
                this.Index = 0;
            }
        }

        private byte[] ObjBytes;
        private int Index = 0;
        private Dictionary<int, object> ObjectIdDict = new Dictionary<int, object>();

        public T ReadObject<T>(Action<SmartObjectDeserializer, T> ReadFunc)
            where T : new()
        {
            int id = this.ReadInt32();
            if (id == 0)
                return default(T);

            T obj;
            if (!this.ObjectIdDict.ContainsKey(id))
            {
                obj = new T();
                this.ObjectIdDict[id] = obj;
                ReadFunc(this, obj);
            }
            else
                obj = (T)this.ObjectIdDict[id];
            return obj;
        }

        public List<T> ReadObjectList<T>(Action<SmartObjectDeserializer, T> ReadFunc)
            where T : new()
        {
            int objCount = this.ReadInt32();
            List<T> objList = new List<T>(objCount);
            for (int i = 0; i < objCount; i++)
            {
                T obj = this.ReadObject(ReadFunc);
                objList.Add(obj);
            }
            return objList;
        }

        public List<T> ReadValueList<T>(Func<SmartObjectDeserializer, T> ReadFunc)
        {
            int valCount = this.ReadInt32();
            List<T> valList = new List<T>(valCount);
            for (int i = 0; i < valCount; i++)
            {
                T val = ReadFunc(this);
                valList.Add(val);
            }
            return valList;
        }

        public Dictionary<TKey, TValue> ReadObjectDic<TKey, TValue>(Func<SmartObjectDeserializer, TKey> readKeyFunc, Func<SmartObjectDeserializer, TValue> readValueFunc)
        {
            int objCount = this.ReadInt32();
            Dictionary<TKey, TValue> objDic = new Dictionary<TKey, TValue>();

            for (int i = 0; i < objCount; i++)
            {
                TKey key = readKeyFunc(this);
                TValue value = readValueFunc(this);
                objDic.Add(key, value);
            }
            return objDic;
        }

        #region 引用类型
        public byte[] ReadBytes()
        {
            bool isNotNull = this.ReadBool();
            if (!isNotNull) return null;

            int bytesLength = this.ReadInt32();
            byte[] val = new byte[bytesLength];
            Array.Copy(this.ObjBytes, this.Index, val, 0, bytesLength);

            this.Index += bytesLength;
            return val;
        }

        public string ReadString()
        {
            bool isNotNull = this.ReadBool();
            if (!isNotNull) return null;

            int byteLength = this.ReadInt32();
            int strStartIndex = this.Index;
            this.Index += byteLength;
            return Encoding.Unicode.GetString(this.ObjBytes, strStartIndex, byteLength);
        }
        #endregion

        #region 值类型
        public bool ReadBool()
        {
            bool val = BitConverter.ToBoolean(this.ObjBytes, this.Index);
            this.Index += 1;
            return val;
        }

        public short ReadInt16()
        {
            short val = BitConverter.ToInt16(this.ObjBytes, this.Index);
            this.Index += 2;
            return val;
        }

        public int ReadInt32()
        {
            int val = BitConverter.ToInt32(this.ObjBytes, this.Index);
            this.Index += 4;
            return val;
        }

        public Int64 ReadInt64()
        {
            Int64 val = BitConverter.ToInt64(this.ObjBytes, this.Index);
            this.Index += 8;
            return val;
        }

        public float ReadFloat()
        {
            float val = BitConverter.ToSingle(this.ObjBytes, this.Index);
            this.Index += 4;
            return val;
        }

        public double ReadDouble()
        {
            double val = BitConverter.ToDouble(this.ObjBytes, this.Index);
            this.Index += 8;
            return val;
        }

        public decimal ReadDecimal()
        {
            string val = this.ReadString();
            return decimal.Parse(val);
        }

        public DateTime ReadDateTime()
        {
            long ticks = BitConverter.ToInt64(this.ObjBytes, this.Index);
            this.Index += 8;
            return DateTime.MinValue.AddTicks(ticks);
        }
        #endregion

        #region 可空值类型
        public bool? ReadNullableBool()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            bool val = BitConverter.ToBoolean(this.ObjBytes, this.Index);
            this.Index += 1;
            return val;
        }

        public short? ReadNullableInt16()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            short val = BitConverter.ToInt16(this.ObjBytes, this.Index);
            this.Index += 2;
            return val;
        }

        public int? ReadNullableInt32()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            int val = BitConverter.ToInt32(this.ObjBytes, this.Index);
            this.Index += 4;
            return val;
        }

        public Int64? ReadNullableInt64()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            Int64 val = BitConverter.ToInt64(this.ObjBytes, this.Index);
            this.Index += 8;
            return val;
        }

        public float? ReadNullableFloat()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            float val = BitConverter.ToSingle(this.ObjBytes, this.Index);
            this.Index += 4;
            return val;
        }

        public double? ReadNullableDouble()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            double val = BitConverter.ToDouble(this.ObjBytes, this.Index);
            this.Index += 8;
            return val;
        }

        public decimal? ReadNullableDecimal()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            string val = this.ReadString();
            return decimal.Parse(val);
        }

        public DateTime? ReadNullableDateTime()
        {
            bool hasValue = this.ReadBool();
            if (!hasValue) return null;

            long ticks = BitConverter.ToInt64(this.ObjBytes, this.Index);
            this.Index += 8;
            return DateTime.MinValue.AddTicks(ticks);
        }
        #endregion

        #region Object
        public object ReadObj(Type t)
        {
            if (t == typeof(byte[]))
            {
                return this.ReadBytes();
            }
            else if (t == typeof(string))
            {
                return this.ReadString();
            }
            else if (t == typeof(bool))
            {
                return this.ReadBool();
            }
            else if (t == typeof(bool?))
            {
                return this.ReadNullableBool();
            }
            else if (t == typeof(short))
            {
                return this.ReadInt16();
            }
            else if (t == typeof(short?))
            {
                return this.ReadNullableInt16();
            }
            else if (t == typeof(int))
            {
                return this.ReadInt32();
            }
            else if (t == typeof(int?))
            {
                return this.ReadNullableInt32();
            }
            else if (t == typeof(Int64))
            {
                return this.ReadInt64();
            }
            else if (t == typeof(Int64?))
            {
                return this.ReadNullableInt64();
            }
            else if (t == typeof(float))
            {
                return this.ReadFloat();
            }
            else if (t == typeof(float?))
            {
                return this.ReadNullableFloat();
            }
            else if (t == typeof(double))
            {
                return this.ReadDouble();
            }
            else if (t == typeof(double?))
            {
                return this.ReadNullableDouble();
            }
            else if (t == typeof(decimal))
            {
                return this.ReadDecimal();
            }
            else if (t == typeof(decimal?))
            {
                return this.ReadNullableDecimal();
            }
            else if (t == typeof(DateTime))
            {
                return this.ReadDateTime();
            }
            else if (t == typeof(DateTime?))
            {
                return this.ReadNullableDateTime();
            }
            else
            {
                throw new Exception(string.Format("未识别的反序列化类型[{0}]", t.ToString()));
            }
        }
        #endregion
    }
}
