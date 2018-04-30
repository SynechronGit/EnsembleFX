using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace EnsembleFX.Core.Dependency
{
    /// <summary>
    /// Represents a class to support Microsoft.Practices.Unity functionality.
    /// </summary>
    public class DependencyManager : IDependencyManager
    {
        protected IUnityContainer container;
        private static IDependencyManager current;

        #region Constructor
        /// <summary>
        /// Creates a new instance of DependencyManager by creating a new unity container and loading configuration for "unity section.
        /// </summary>
        public DependencyManager()
        {
            if (container == null)
            {
                container = new UnityContainer();
                UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

                container.LoadConfiguration(section);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="injectedContainer">Injected container into the constructor of DependencyManager</param>
        public DependencyManager(IUnityContainer injectedContainer)
        {
            container = injectedContainer;
        }

        #endregion

        public IUnityContainer Container
        {
            get
            {
                return this.container;
            }
        }

        public static IDependencyManager Current
        {
            get
            {
                if (current != null)
                {
                    current = DependencyManager.CreateInstance();
                }
                return current;
            }
        }

        public static IDependencyManager CreateInstance()
        {
            return new DependencyManager();
        }

        #region Public Methods    
   
        /// <summary>
        /// Resolves the Type t from registration
        /// </summary>
        /// <param name="t">Type to resolve</param>
        /// <returns>Resolved object for Type t</returns>
        public object Resolve(Type t)
        {
            return container.Resolve(t);
        }

        /// <summary>
        /// Resolve an instance of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to get from the container.</typeparam>
        /// <returns>The retrieved object.</returns>
        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
       

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T">The type of object to get from the container.</typeparam>
        /// <param name="name">The name</param>
        /// <returns>The retrieved object.</returns>
        public T Resolve<T>(string name)
        {
            return container.Resolve<T>(name);
        }

        /// <summary>
        ///  Return instances of all registered types requested.
        /// </summary>
        /// <typeparam name="T">The type requested.</typeparam>
        /// <returns>Set of objects of type T.</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <typeparam name="T">The type of instance.</typeparam>
        /// <param name="instance">Object to returned.</param>
        public void RegisterInstance<T>(T instance)
        {
            container.RegisterInstance<T>(instance);
        }

        /// <summary>
        ///  Register an instance with the container.
        /// </summary>
        /// <typeparam name="T">The type requested.</typeparam>
        /// <param name="name">Name for registration.</param>
        /// <param name="instance">Object to returned.</param>
        public void RegisterInstance<T>(string name, T instance)
        {
            container.RegisterInstance<T>(name, instance);
        }

        /// <summary>
        /// Builds the container with the instance and name
        /// </summary>
        /// <param name="instance">Instance to build up</param>
        /// <param name="name">Named parameter for the instance</param>
        /// <returns>Object after BuildUp</returns>
        public object Build(object instance, string name="")
        {
            if (string.IsNullOrEmpty(name))
            {
                return container.BuildUp(instance.GetType(), instance);
            }
            else
            {
                return container.BuildUp(instance.GetType(), instance, name);
            }
        }
        #endregion
    }
}
