using Ejab.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.UI.IocConfig;
using Ejab.BAL.Services.Questions;
using Ejab.BAL.Services.Emailes;
using Ejab.BAL.Services.statistics;
using System.Web.Security;
using Ejab.UI.Models;
using Ejab.BAL.Services.AboutUs;
using Ejab.BAL.Services.AboutApp;
using Ejab.BAL.Services.Notification;
using Ejab.UI.Helpers;
using Ejab.BAL.Services.Reports;

namespace Ejab.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.DefaultNamespaces.Add("Ejab.UI.Controllers");
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var unity = new UnityContainer();
            unity.RegisterType<IUnitOfWork, UnitOfWork>();
            unity.RegisterType<IRuleService, RuleService>();            
            unity.RegisterType<IUserService, UserService>();           
            unity.RegisterType<ICustomerService, CustomerService>();
            unity.RegisterType<HomeController>();
            unity.RegisterType<CustomerController>();
            unity.RegisterType<IOfferService, OfferService>();
            unity.RegisterType<OfferController>();
            unity.RegisterType<IRegionService, RegionService>();
            unity.RegisterType<IRequestService, RequestService>();
            unity.RegisterType<IComplaintService, ComplaintService>();
            unity.RegisterType<IMessage, MessageService>();
            unity.RegisterType<ITruckService, TruckService>();
            unity.RegisterType<ITruckTypeService, TruckTypeServicecs>();
            unity.RegisterType<IQuestionService, QuestionService>();
            unity.RegisterType<IAboutUs, AboutUsService>();
            unity.RegisterType<IAboutAppService, AboutAppService>();
            unity.RegisterType<IstatisticsService, statisticsService>();
            unity.RegisterType<INotificationService, NotificationService>();
            unity.RegisterType<IReportService, ReportService>();
            unity.RegisterType<RequestController>();
            unity.RegisterType<RuleController>();
            unity.RegisterType<MessageController>();
            unity.RegisterType<ComplaintController>();
            unity.RegisterType<TruckTypeController>();
            unity.RegisterType<TruckController>();
            unity.RegisterType<RegionController>();
            unity.RegisterType<QuestionController>();
            unity.RegisterType<IEmaileService, EmaileService>();
            unity.RegisterType<EmailController>();         
            unity.RegisterType<StatisticsController>();     
            unity.RegisterType<AboutUsController>();
            unity.RegisterType<AboutAppController>();           
            unity.RegisterType<NotifyController>();
            unity.RegisterType<ChartController>();
            unity.RegisterType<AdminController>();
            unity.RegisterType<Isettingcs, SettingService>();
            unity.RegisterType<SettingController>();
            unity.RegisterType<ReportsController>();
            // Finally, override the default dependency resolver with Unity
            //HttpConfiguration .Configuration.DependencyResolver = new IocConfig .ScopeContainer.IoCContainer(unity);
            DependencyResolver.SetResolver(new UnityDependencyResolver(unity));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie langCookie = HttpContext.Current.Request.Cookies["language"];
               if (langCookie != null && langCookie.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(langCookie.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCookie.Value);
            }
            else
            {

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-EG");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-EG");
            }
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
          
            HttpCookie authCookie =
                          Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                FormsIdentity id = new FormsIdentity(authTicket);
                // This principal will flow throughout the request.
                string[] roles = new string[] { "admin" };
                MyPrincipal UserDTO = new MyPrincipal(id, roles );

                Context.User = UserDTO;
            }
           
           
        }
    }
}
