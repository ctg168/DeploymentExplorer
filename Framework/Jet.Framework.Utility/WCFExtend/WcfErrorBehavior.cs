using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.IO;

namespace Jet.Framework.Utility
{
    public class WcfErrorBehavior : IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            try
            {
                FaultReason faultReason = new FaultReason(error.Message);
                ExceptionDetail exceptionDetail = new ExceptionDetail(error);
                FaultCode faultCode = FaultCode.CreateSenderFaultCode(new FaultCode("0"));
                FaultException<ExceptionDetail> faultException = new FaultException<ExceptionDetail>(exceptionDetail, faultReason, faultCode);
                MessageFault messageFault = faultException.CreateMessageFault();
                fault = Message.CreateMessage(version, messageFault, faultException.Action);
            }
            catch
            {
                // Todo log error
            }
        }


        public bool HandleError(Exception ex)
        {
            try
            {
                Debug.WriteLine(ex.ToString());
            }
            catch
            {
                // Todo log error
            }
            return true;
        }
    }
}
