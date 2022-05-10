using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Ejab.Rest.Services
{
    public class ControllerSelector :DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;

        public ControllerSelector(HttpConfiguration config):base(config)
        {
            _config = config;
        }
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
          
                var Controllers = GetControllerMapping();
            var routedata = request.GetRouteData();
            var controllerName =(string)routedata.Values["controller"];
            HttpControllerDescriptor ControllerDescriptor;
            if (Controllers.TryGetValue(controllerName, out ControllerDescriptor))
            {
                    var Version = "1";
                    var newName = string.Concat(controllerName, "V", Version);
                    HttpControllerDescriptor Versioningdescriptor;
                    if (Controllers.TryGetValue(newName,out Versioningdescriptor))
                    {
                        return Versioningdescriptor;
                    }
                return ControllerDescriptor;
            }
            return null;
           
        }
    }
}