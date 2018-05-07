using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace EnsembleFX.Core.Dependency
{
    /// <summary>
    /// 
    /// </summary>
    //public class DependencyInstanceProvider //: IInstanceProvider //To Do : .NET Core replacement for IInstanceProvider
    //{

    //    #region Constructors

    //    public DependencyInstanceProvider() : this(null) 
    //    { 
            
    //    } 

    //    public DependencyInstanceProvider(Type type)
    //    {
    //        ServiceType = type;
    //        dependencyManager = new DependencyManager();
    //    }

    //    #endregion

    //    #region Private Members
        
    //    private IDependencyManager dependencyManager;

    //    #endregion

    //    #region Public Properties

    //    public Type ServiceType { set; get; }

    //    #endregion

    //    #region IInstanceProvider Members

    //    public object GetInstance(InstanceContext instanceContext, Message message)
    //    {
    //        return dependencyManager.Resolve(ServiceType);
    //    }

    //    public object GetInstance(InstanceContext instanceContext)
    //    {
    //        return GetInstance(instanceContext, null); 
    //    }

    //    public void ReleaseInstance(InstanceContext instanceContext, object instance)
    //    {

    //    }

    //    #endregion
    //}
}
