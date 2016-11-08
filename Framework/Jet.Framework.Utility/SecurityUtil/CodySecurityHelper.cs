using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class CodySecurityHelper
    {
        internal const byte VersionNO1 = 1;//加密版本号        

        /// <summary>
        /// 加密(经测试10万次加密解密10K数据,2秒),目前使用加密版本1加密
        /// </summary>
        public static byte[] Encrypt(byte[] bytes, int key)
        {
            List<byte> byteList = new List<byte>();
            byteList.Add(CodySecurityHelper.VersionNO1);

            CodySecurityBase codySecurity = CodySecurityFactory.CreateCodySecurity(CodySecurityHelper.VersionNO1);
            codySecurity.Encrypt(byteList, bytes, key);

            return byteList.ToArray();
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static byte[] Decrypt(byte[] bytes, int key)
        {
            if (bytes == null || bytes.Length == 0)
                return new byte[0];

            byte versionNO = bytes[0];
            CodySecurityBase codySecurity = CodySecurityFactory.CreateCodySecurity(versionNO);
            return codySecurity.Decrypt(bytes, 1, key);
        }

        public static byte[] CompressAndEncrypt(byte[] bytes, int key)
        {
            return CodySecurityHelper.Encrypt(SharpZipHelper.Compress(bytes), key);
        }

        public static byte[] DecryptAndDecompress(byte[] bytes, int key)
        {
            return SharpZipHelper.Decompress(CodySecurityHelper.Decrypt(bytes, key));
        }

        public static byte[] StringToBytes(string str, int key)
        {
            if (str == null)
                return new byte[0];

            return CodySecurityHelper.CompressAndEncrypt(Encoding.UTF8.GetBytes(str), key);
        }

        public static string BytesToString(byte[] bytes, int key)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            var newbytes = CodySecurityHelper.DecryptAndDecompress(bytes, key);
            if (newbytes == null)
                return string.Empty;
            else
                return Encoding.UTF8.GetString(newbytes, 0, newbytes.Length);
        }
    }
}
