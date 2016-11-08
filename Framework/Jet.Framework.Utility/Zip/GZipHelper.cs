using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Jet.Framework.Utility
{
    public static class GZipHelper
    {        
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        public static byte[] Compress(byte[] bt)
        {
            MemoryStream ms = new MemoryStream();

            GZipStream s = new GZipStream(ms, CompressionMode.Compress, true);

            s.Write(bt, 0, bt.Length);

            s.Close();
            ms.Close();

            return ms.ToArray();
        }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream();
            input.Write(data, 0, data.Length);
            input.Position = 0;
            GZipStream gzip = new GZipStream(input,
                              CompressionMode.Decompress, true);
            MemoryStream output = new MemoryStream();
            byte[] buff = new byte[64];
            int read = -1;
            read = gzip.Read(buff, 0, buff.Length);
            while (read > 0)
            {
                output.Write(buff, 0, read);
                read = gzip.Read(buff, 0, buff.Length);
            }

            byte[] result = output.ToArray();

            output.Close();
            gzip.Close();

            return result;
        }
    }
}
