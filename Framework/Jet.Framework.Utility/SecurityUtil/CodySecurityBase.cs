using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public abstract class CodySecurityBase
    {
        protected byte HeadLength { get; set; }

        public CodySecurityBase(byte headLength)
        {
            this.HeadLength = headLength;
        }

        protected abstract byte[] CreateHead(int key);        
        protected abstract bool ValidateHead(byte[] headBytes, int key);
        protected abstract byte[] GetValidateErrInfo(byte[] headBytes);       

        public void Encrypt(List<byte> byteList, byte[] bytes, int key)
        {
            byteList.AddRange(this.CreateHead(key));
            if (bytes != null)
                byteList.AddRange(bytes);
        }

        public byte[] Decrypt(byte[] bytes, int startIndex, int key)
        {
            byte[] headBytes = new byte[this.HeadLength];
            Array.Copy(bytes, startIndex, headBytes, 0, this.HeadLength);

            if (!this.ValidateHead(headBytes, key))
                return this.GetValidateErrInfo(headBytes);

            int bodyLength = bytes.Length - this.HeadLength - startIndex;
            if (bodyLength > 0)
            {
                byte[] bodyBytes = new byte[bodyLength];
                Array.Copy(bytes, startIndex + this.HeadLength, bodyBytes, 0, bodyLength);
                return bodyBytes;
            }
            else
            {
                return new byte[0];
            }
        }
    }
}
