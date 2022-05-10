using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Ejab.UI.IocConfig
{
    internal class UnityDependencyResolver : IDependencyResolver
    {
        private UnityContainer unity;

        public UnityDependencyResolver(UnityContainer unity)
        {
            this.unity = unity;
        }

        public object GetService(Type serviceType)
        {
            if (unity.IsRegistered(serviceType))
            {
                return unity.Resolve(serviceType);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (unity.IsRegistered(serviceType))
            {
                return unity.ResolveAll(serviceType);
            }
            else
            {
                return new List<object>();
            }
        }
    }
}