using System;
using System.Net;
using System.Windows;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Jet.Framework.Utility
{
    public static class ServiceClientFactory<TServiceClient, TService>
        where TServiceClient : ClientBase<TService>, TService
        where TService : class
    {
        public static TServiceClient CreateServiceClient(Uri serverAddress, bool showbusyui)
        {
            EndpointAddress endpointAddress = new EndpointAddress(serverAddress);

            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;

            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            binding.SendTimeout = new TimeSpan(1, 0, 0);
            binding.ReceiveTimeout = new TimeSpan(1, 0, 0);

            var ctor = typeof(TServiceClient).GetConstructor(new Type[] { typeof(Binding), typeof(EndpointAddress) });
            var clientService = (TServiceClient)ctor.Invoke(new object[] { binding, endpointAddress });

            clientService.Endpoint.Behaviors.Add(new ClientEndpointBehavior() { ShowBusyUI = showbusyui });
                        
            return clientService;
        }
    }
}
