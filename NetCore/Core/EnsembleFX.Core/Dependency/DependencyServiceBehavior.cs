using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;

namespace EnsembleFX.Core.Dependency
{
    /// <summary>
    /// Dependency injection Service Behavior
    /// </summary>
    //public class DependencyServiceBehavior //: IServiceBehavior //To Do : .NET Core replacement for IServiceBehavior. Uncomment the commented code.
    //{

    //    //#region Constructors

    //    //public DependencyServiceBehavior()
    //    //{
    //    //    InstanceProvider = new DependencyInstanceProvider();
    //    //}

    //    //#endregion

    //    //#region Private Members
        
    //    //private ServiceHost serviceHost = null;

    //    //#endregion

    //    //#region Public Properties

    //    //public DependencyInstanceProvider InstanceProvider { get; set; }

    //    //#endregion

    //    //#region IServiceBehavior Implementation

    //    //public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    //    //{
            
    //    //}

    //    //public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
    //    //{
    //    //}

    //    //public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    //    //{
    //    //    foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
    //    //    {
    //    //        ChannelDispatcher cd = cdb as ChannelDispatcher;
    //    //        if (cd != null)
    //    //        {
    //    //            foreach (EndpointDispatcher ed in cd.Endpoints)
    //    //            {
    //    //                InstanceProvider.ServiceType = serviceDescription.ServiceType;
    //    //                ed.DispatchRuntime.InstanceProvider = InstanceProvider; 
    //    //            }
    //    //        }
    //    //    }
    //    //}

    //    //#endregion
    //}
}
