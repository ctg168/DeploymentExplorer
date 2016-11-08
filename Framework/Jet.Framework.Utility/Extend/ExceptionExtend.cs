using System;
using System.Net;
using System.Windows;
using System.ServiceModel;

namespace Jet.Framework.Utility
{
    public static class ExceptionExtend
    {
        public static bool IsApplicationException(this Exception ex)
        {
            var err = ex as FaultException<ExceptionDetail>;
            if (null != err)
            {
                return err.Detail.Type.Equals("System.ApplicationException");
            }
            return false;
        }

        public static bool IsCommunicationException(this Exception ex)
        {
            return ex.GetType() == typeof(CommunicationException);
        }
    }
}
