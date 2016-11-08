using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Jet.Framework.Utility
{
    public class WcfSilverlightFaultBehavior : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply.IsFault)
            {
                HttpResponseMessageProperty property = new HttpResponseMessageProperty();
                property.StatusCode = System.Net.HttpStatusCode.OK;
                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }
    }
}
