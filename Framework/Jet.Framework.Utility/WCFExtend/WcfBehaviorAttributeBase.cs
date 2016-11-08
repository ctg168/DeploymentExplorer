using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Jet.Framework.Utility
{
   public class WcfBehaviorAttributeBase:Attribute,IServiceBehavior 
    {
        private Type _behaviorType;       
        public WcfBehaviorAttributeBase(Type typeBehavior)
        {
            _behaviorType = typeBehavior;
        }
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            
        }
       public  void ApplyDispatchBehavior(ServiceDescription serviceDescription,
            System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            object behavior;
            try
            {
                behavior = Activator.CreateInstance(_behaviorType);
            }
            catch (MissingMethodException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException(e.Message);
            }
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                if (behavior is IParameterInspector)
                {
                    foreach (EndpointDispatcher epDisp in channelDispatcher.Endpoints)
                    {
                        foreach (DispatchOperation op in epDisp.DispatchRuntime.Operations)
                            op.ParameterInspectors.Add((IParameterInspector)behavior);
                    }
                }
                else if (behavior is IErrorHandler)
                {
                    channelDispatcher.ErrorHandlers.Add((IErrorHandler)behavior);
                }
                else if (behavior is IDispatchMessageInspector)
                {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add((IDispatchMessageInspector)behavior);
                    }
                }
            }
        }
       public void Validate(ServiceDescription serviceDescription,
            System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

    }
}
