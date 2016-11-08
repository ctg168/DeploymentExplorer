using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Jet.Framework.Utility
{
    //压缩测试 34M -> 7M,135418B -> 20138B,47617B -> 11655B,441371B -> 108265B
    public partial class SharpZipHelper
    {
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        public static void Compress(Stream stream, byte[] bytes, int offset = 0)
        {
            if (bytes == null || bytes.Length == 0) return;

            ZipOutputStream zipOutputStream = new ZipOutputStream(stream);
            ZipEntry entry = new ZipEntry("");
            zipOutputStream.PutNextEntry(entry);
            zipOutputStream.Write(bytes, offset, bytes.Length - offset);
            zipOutputStream.Finish();
            zipOutputStream.Close();
        }

        /// <summary>
        /// 压缩字节数组
        /// </summary>
        public static byte[] Compress(byte[] bytes, int offset = 0)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SharpZipHelper.Compress(ms, bytes, offset);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 压缩字节数组
        /// </summary>
        public static void Compress(string desFilePath, string tagetFilePath)
        {
            byte[] bytes = File.ReadAllBytes(desFilePath);

            if (File.Exists(tagetFilePath))
                throw new Exception("目标文件已存在!");

            using (FileStream fs = File.Create(tagetFilePath))
            {
                SharpZipHelper.Compress(fs, bytes, 0);
            }
        }

        ///// <summary>
        ///// 压缩字节数组
        ///// </summary>
        //public static byte[] Compress(byte[] bytes, int offset = 0)
        //{
        //    if (bytes == null || bytes.Length == 0) return new byte[0];

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        ZipOutputStream zipOutputStream = new ZipOutputStream(ms);
        //        ZipEntry entry = new ZipEntry("");
        //        zipOutputStream.PutNextEntry(entry);
        //        zipOutputStream.Write(bytes, offset, bytes.Length - offset);
        //        zipOutputStream.Finish();
        //        zipOutputStream.Close();
        //        return ms.ToArray();
        //    }
        //}

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        public static byte[] Decompress(byte[] bytes, int offset, int count)
        {
            if (bytes == null || bytes.Length == 0) return new byte[0];

            using (MemoryStream ms = new MemoryStream(bytes, offset, count))
            {
                ZipInputStream zipInputStream = new ZipInputStream(ms);
                ZipEntry entry = zipInputStream.GetNextEntry();
                byte[] bytesTemp = new byte[entry.Size];
                zipInputStream.Read(bytesTemp, 0, bytesTemp.Length);
                zipInputStream.Close();

                return bytesTemp;
            }
        }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        public static byte[] Decompress(byte[] bytes, int offset = 0)
        {
            return SharpZipHelper.Decompress(bytes, offset, bytes.Length - offset);
        }

        public static byte[] CompressString(string str, Encoding encoding)
        {
            if (string.IsNullOrEmpty(str))
                return new byte[0];

            return SharpZipHelper.Compress(encoding.GetBytes(str));
        }

        public static byte[] CompressString(string str)
        {
            return SharpZipHelper.CompressString(str, Encoding.Unicode);
        }

        public static string DecompressString(byte[] bytes, Encoding encoding)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            return encoding.GetString(SharpZipHelper.Decompress(bytes));
        }

        public static string DecompressString(byte[] bytes)
        {
            return SharpZipHelper.DecompressString(bytes, Encoding.Unicode);
        }
    }
}
