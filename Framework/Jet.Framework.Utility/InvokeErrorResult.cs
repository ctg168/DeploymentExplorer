using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Jet.Framework.Utility
{
    [DataContract(IsReference = true)]
    public class InvokeErrorResult
    {
        public InvokeErrorResult()
        {

        }

        public InvokeErrorResult(byte errorCode, string errormessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errormessage;
        }

        public static InvokeErrorResult CreateSuccessResult()
        {
            return new InvokeErrorResult(0, null);
        }

        [DataMember]
        public byte ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
