using System;
using System.Net;
using System.Text;

namespace Jet.Framework.Utility
{
    public abstract class CodySafetyBase<T>
        where T : CustomerInfoBase
    {
        public abstract T ReadCustomerInfo(int codySystemType, out string error);
        public abstract T ReadCustomerInfo(out string error);

        public abstract void WriteCustomerInfo(T info, out string error);
    }
}
