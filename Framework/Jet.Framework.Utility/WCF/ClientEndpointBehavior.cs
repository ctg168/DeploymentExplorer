using System;
using System.Net;
using System.Windows;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Jet.Framework.Utility
{
    public class ClientEndpointBehavior : IEndpointBehavior
    {
        public ClientEndpointBehavior()
        {
            
        }

        public bool ShowBusyUI { get; set; }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //clientRuntime.MessageInspectors.Add(new ClientMessageInspector() { ShowBusyUI = this.ShowBusyUI});
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }
}
