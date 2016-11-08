using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jet.Framework.Utility
{
    public class CodySecurityHelper2
    {
        public const string DefaultKey = "CodyMaster";//不超过16个字符
        public const string DefaultIV = "Jet";//不超过8个字符
        private string Key;
        private string IV;
        private ICryptoTransform Encryptor;
        private ICryptoTransform Decryptor;

        public CodySecurityHelper2(string key = CodySecurityHelper2.DefaultKey, string iv = CodySecurityHelper2.DefaultIV)
        {
            this.Key = key;
            this.IV = iv;
            RC2 rc = RC2.Create();

            key = key.PadLeft(16, ' ');
            key = key.Substring(key.Length - 16, 16);

            iv = iv.PadLeft(8, ' ');
            iv = iv.Substring(iv.Length - 8, 8);

            this.Encryptor = rc.CreateEncryptor(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(iv));
            this.Decryptor = rc.CreateDecryptor(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(iv));
        }

        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public byte[] Compress(byte[] bt)
        {
            return GZipHelper.Compress(bt);
        }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decompress(byte[] data)
        {
            return GZipHelper.Decompress(data);
        }
        /// <summary>
        /// 加密
        /// </summary>
        public byte[] Encrypt(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return new byte[0];
            }

            byte[] EncryptDatas = Encryptor.TransformFinalBlock(data, 0, data.Length);
            return EncryptDatas;
        }

        /// <summary>
        /// 解密
        /// </summary>        
        public byte[] Decrypt(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return new byte[0];
            }

            byte[] DecryptDatas = Decryptor.TransformFinalBlock(data, 0, data.Length);
            return DecryptDatas;
        }

        public byte[] TexttoByteWithZipEncrypt(string text)
        {
            return BytetoByteWithZipEncrypt(Encoding.Default.GetBytes(text));
        }

        public string BytetoTextWithUnzipDecrypt(byte[] Byte)
        {
            return Encoding.Default.GetString(BytetoByteWithUnzipDecrypt(Byte));
        }

        public byte[] BytetoByteWithZipEncrypt(byte[] data)
        {
            return this.Encrypt(this.Compress(data));
        }

        public byte[] BytetoByteWithUnzipDecrypt(byte[] data)
        {
            return this.Decompress(this.Decrypt(data));
        }
    }
}
