using Ejab.BAL.Reository;
using Ejab.BAL.Services;
using Ejab.BAL.Services.AboutApp;
using Ejab.BAL.Services.AboutUs;
using Ejab.BAL.Services.Notification;
using Ejab.BAL.Services.SMS;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using Ejab.Rest.Controllers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Ejab.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          //  Database.SetInitializer<EjabContext>(new DropCreateDatabaseIfModelChanges<EjabContext>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var unity = new UnityContainer();

            // Register the Controllers that should be injectable
            // i will register all my controllers
            unity.RegisterType<IUnitOfWork, UnitOfWork>();
            unity.RegisterType<IUserService, UserService>();
            unity.RegisterType<IRuleService, RuleService>();
            unity.RegisterType<TrucksV1Controller>();
            unity.RegisterType<TruckTypeV1Controller>();
           
            unity.RegisterType<OfferV1Controller>();
            unity.RegisterType<OfferImagesController>();
            unity.RegisterType<ServiceTypeV1Controller>();
            unity.RegisterType<MessageV1Controller>();
            unity.RegisterType<SuggestionsComplaintV1Controller>();
            unity.RegisterType<RequestV1Controller>();
            unity.RegisterType<MessageV1Controller>();
            unity.RegisterType<InterestV1Controller>();            
            unity.RegisterType<SuggestionsComplaintV1Controller>();
            unity.RegisterType<RegionV1Controller>();
            // Register instances to be used when resolving constructor parameter dependencies
            // here i will register  my repository and unit of work
            unity.RegisterType<EjabContext, EjabContext>();
            unity.RegisterType<IRegionService, RegionService>();
            unity.RegisterType<ICustomerService, CustomerService>();
            unity.RegisterType<CustomerV1Controller>();
            unity.RegisterType<IRequestService, RequestService>();
            unity.RegisterType<IMessage, MessageService>();
            unity.RegisterType<IComplaintService, ComplaintService>();
            unity.RegisterType<IOfferService, OfferService>();
            unity.RegisterType<IServiceType, ServiceTypeService>();
            unity.RegisterType<ITruckTypeService, TruckTypeServicecs>();
            unity.RegisterType<ITruckService, TruckService>();
            unity.RegisterType<IIntersetService, Interestservice>();
            unity.RegisterType<IRegionService, RegionService>();
            unity.RegisterType<INotificationService, NotificationService>();
            unity.RegisterType<NotificationV1Controller>();
            unity.RegisterType<IAboutUs, AboutUsService>();
            unity.RegisterType<AboutUsController>();
            unity.RegisterType<IAboutAppService, AboutAppService>();
            unity.RegisterType<AboutAppV1Controller>();
            unity.RegisterType<ISmsService, SmsService>();
            unity.RegisterType<SmsMessageV1Controller>();
            unity.RegisterType(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Finally, override the default dependency resolver with Unity
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyConfig.ScopeContainer.IoCContainer(unity);
        }
    }
}
