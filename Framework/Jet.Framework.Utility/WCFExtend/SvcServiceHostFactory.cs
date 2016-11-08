using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Jet.Framework.Utility
{
    public class SvcServiceHostFactory : ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost serviceHost = new ServiceHost(serviceType, baseAddresses);

            foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new HttpFaultBehavior());

                foreach (OperationDescription operationDescription in endpoint.Contract.Operations)
                {

                }
            }
            return serviceHost;
        }
    }
}
