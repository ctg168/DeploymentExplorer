using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace Jet.Framework.Utility
{
    /// <summary>
    /// 配置 WCF SOAP 错误以用于 Silverlight 客户端
    /// http://msdn.microsoft.com/zh-cn/library/ee844556(v=vs.95).aspx
    /// </summary>
    public class HttpFaultMessageInspector : IDispatchMessageInspector
    {
        object IDispatchMessageInspector.AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Do nothing to the incoming message
            return null;
        }

        void IDispatchMessageInspector.BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (null == reply) { return; }

            if (reply.IsFault)
            {
                HttpResponseMessageProperty property = new HttpResponseMessageProperty();
                property.StatusCode = System.Net.HttpStatusCode.OK; // 200

                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }
    }
}
