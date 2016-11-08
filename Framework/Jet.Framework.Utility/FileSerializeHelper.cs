using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Jet.Framework.Utility
{
    public class FileSerializeHelper
    {
        public static void WriteBytes(Stream fs, byte[] bytes)
        {
            if (bytes.Length > 0)
                fs.Write(bytes, 0, bytes.Length);
        }

        public static void WriteByteArray(Stream fs, byte[] bytes)
        {
            if (bytes == null)
            {
                bytes = new byte[0];
            }
            FileSerializeHelper.WriteInt32(fs, bytes.Length);
            FileSerializeHelper.WriteBytes(fs, bytes);
        }

        public static void WriteBool(Stream fs, bool val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            FileSerializeHelper.WriteBytes(fs, bytes);
        }

        public static void WriteInt32(Stream fs, int val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            FileSerializeHelper.WriteBytes(fs, bytes);
        }

        public static void WriteString(Stream fs, string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            FileSerializeHelper.WriteInt32(fs, bytes.Length);
            FileSerializeHelper.WriteBytes(fs, bytes);
        }

        public static byte[] ReadByteArray(Stream fs)
        {
            int bytesLength = FileSerializeHelper.ReadInt32(fs);

            if (bytesLength > 0)
            {
                return FileSerializeHelper.ReadBytes(fs, bytesLength);
            }
            else
            {
                return new byte[0];
            }
        }

        public static byte[] ReadBytes(Stream fs, int bytesLength)
        {
            byte[] bytes = new byte[bytesLength];
            int leaveLength = bytesLength;//需要读取的字节数
            int offset = 0;//偏移量
            do
            {
                int readLength = fs.Read(bytes, offset, leaveLength);
                leaveLength -= readLength;
                offset += readLength;
            }
            while (leaveLength > 0);

            return bytes;
        }

        public static bool ReadBool(Stream fs)
        {
            byte[] bytes = FileSerializeHelper.ReadBytes(fs, 1);
            return BitConverter.ToBoolean(bytes, 0);
        }

        public static int ReadInt32(Stream fs)
        {
            byte[] bytes = FileSerializeHelper.ReadBytes(fs, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static string ReadString(Stream fs)
        {
            int bytesLength = FileSerializeHelper.ReadInt32(fs);
            if (bytesLength > 0)
            {
                byte[] bytes = FileSerializeHelper.ReadBytes(fs, bytesLength);
                return Encoding.Unicode.GetString(bytes, 0, bytesLength);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
