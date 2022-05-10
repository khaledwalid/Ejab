using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Ejab.Rest.DependencyConfig
{
    public class ScopeContainer : IDependencyScope
    {
        protected IUnityContainer container;
        public ScopeContainer(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public void Dispose()
        {
            container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            if (container.IsRegistered(serviceType))
            {
                return container.Resolve(serviceType);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container.IsRegistered(serviceType))
            {
                return container.ResolveAll(serviceType);
            }
            else
            {
                return new List<object>();
            }
        }

        public class IoCContainer : ScopeContainer, IDependencyResolver
        {
            public IoCContainer(IUnityContainer container)
                : base(container)
            {
            }

            public IDependencyScope BeginScope()
            {
                var child = container.CreateChildContainer();
                return new ScopeContainer(child);
            }
        }
    }
}