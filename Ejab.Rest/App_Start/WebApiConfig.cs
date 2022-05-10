using Ejab.Rest.Filters;
using Ejab.Rest.Services;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using WebApiContrib.Formatting.Jsonp;

namespace Ejab.Rest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Filters.Add(new FIAAuthenticationFilter(true));
            // this must be uncommented to enable SSl
            //  config.Filters.Add(new RequireHttpsAttrubite());
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            var formatter = new JsonpMediaTypeFormatter(jsonFormatter, "cb");
            config.Formatters.Insert(0, jsonFormatter);
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/V1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Versioning
            config.Routes.MapHttpRoute(
               name: "User",
               routeTemplate: "api/V1/{controller}/{action}/{id}",
               defaults: new { controller = "CustomerV1", id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
             name: "ServiceType",
             routeTemplate: "api/V1/{controller}/{id}",
             defaults: new { controller = "ServiceTypeV1", id = RouteParameter.Optional }
         );
            config.Routes.MapHttpRoute(
               name: "TruckType",
               routeTemplate: "api/V1/{controller}/{id}",
               defaults: new { controller = "TruckTypeV1", id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
              name: "Trucks",
              routeTemplate: "api/V1/{controller}/{id}",
              defaults: new { controller = "TrucksV1", id = RouteParameter.Optional }
          );
            config.Routes.MapHttpRoute(
             name: "Offer",
             routeTemplate: "api/V1/{controller}/{id}",
             defaults: new { controller = "OfferV1", id = RouteParameter.Optional }
         );
            config.Routes.MapHttpRoute(
           name: "Request",
          routeTemplate: "api/V1/{controller}/{id}",
           defaults: new { controller = "RequestV1", id = RouteParameter.Optional }

       );
            config.Routes.MapHttpRoute(
         name: "Message",
        routeTemplate: "api/V1/{controller}/{id}",
         defaults: new { controller = "MessageV1", id = RouteParameter.Optional }

     );
            config.Routes.MapHttpRoute(
       name: "SuggestionsComplaint",
      routeTemplate: "api/V1/{controller}/{id}",
       defaults: new { controller = "SuggestionsComplaintV1", id = RouteParameter.Optional }

   );
            config.Routes.MapHttpRoute(
      name: "Interest",
     routeTemplate: "api/V1/{controller}/{id}",
      defaults: new { controller = "InterestV1", id = RouteParameter.Optional }

  );

            //config.Routes.MapHttpRoute(
            //         name: "Request",
            //        routeTemplate: "api/V1/{controller}/{id}",
            //         defaults: new { controller = "Request", id = RouteParameter.Optional }

            //     );


         //  config.Services.Replace(typeof(IHttpControllerSelector), new ControllerSelector(config));


        }
    }
}
