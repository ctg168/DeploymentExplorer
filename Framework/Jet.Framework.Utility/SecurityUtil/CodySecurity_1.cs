using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    /// <summary>
    /// 加密类(版本1)
    /// </summary>
    public class CodySecurity_1 : CodySecurityBase
    {
        public const string ValidateErrRtf = "{\\rtf1\\fbidis\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052{\\fonttbl{\\f0\\fnil\\fprq2\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}{\\f1\\fnil\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}}\n{\\colortbl ;\\red255\\green0\\blue0;}\n\\viewkind4\\uc1\\pard\\ltrpar\\nowidctlpar\\qj\\cf1\\lang2052\\f0\\fs21\\'ca\\'fd\\'be\\'dd\\'bd\\'e2\\'c3\\'dc\\'ca\\'a7\\'b0\\'dc\\'a3\\'ac\\'c7\\'eb\\'c1\\'aa\\'cf\\'b5\\'b9\\'a9\\'d3\\'a6\\'c9\\'cc\\'a3\\'a1 (【CustomerKey】)\\cf0\\f1\\fs18\\par\n}\n";
        public const string ValidateErrHeadRtf = "{\\rtf1\\fbidis\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052{\\fonttbl{\\f0\\fnil\\fprq2\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}{\\f1\\fnil\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}}\n{\\colortbl ;\\red255\\green0\\blue0;}\n\\viewkind4\\uc1\\pard\\ltrpar\\nowidctlpar\\qj\\cf1\\lang2052\\f0\\fs21\\'ca\\'fd\\'be\\'dd\\'bd\\'e2\\'c3\\'dc\\'ca\\'a7\\'b0\\'dc\\'a3\\'ac\\'c7\\'eb\\'c1\\'aa\\'cf\\'b5\\'b9\\'a9\\'d3\\'a6\\'c9\\'cc\\'a3\\'a1 (";
        public CodySecurity_1()
            : base(sizeof(int))
        {

        }

        protected override byte[] CreateHead(int key)
        {
            return BitConverter.GetBytes(key);
        }

        protected override bool ValidateHead(byte[] headBytes, int key)
        {
            int currentKey = BitConverter.ToInt32(headBytes, 0);
            return currentKey == key;
        }

        protected override byte[] GetValidateErrInfo(byte[] headBytes)
        {
            int currentKey = BitConverter.ToInt32(headBytes, 0);
            string errRtf = CodySecurity_1.ValidateErrRtf.Replace("【CustomerKey】", currentKey.ToString());
            return SharpZipHelper.Compress(Encoding.UTF8.GetBytes(errRtf));
        }
    }
}
