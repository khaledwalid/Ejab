using Ejab.UI.Helpers;
using Ejab.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{ 
    public class BaseController : Controller
    {
        public MyPrincipal _User
        {
            get
            {
                if (User.GetType() == typeof(MyPrincipal))
                    return (MyPrincipal)User;
                else
                    return null;
            }
        }
        //protected override void ExecuteCore()
        //{
        //    int culture = 0;
        //    if (this.Session == null || this.Session["CurrentCulture"] == null)
        //    {
        //        this.Session["CurrentCulture"] = culture;
        //    }
        //    else
        //    {
        //        culture = (int)this.Session["CurrentCulture"];
        //    }
        //    // calling CultureHelper class properties for setting  
        //    CultureHelper.CurrentCulture = culture;

        //    base.ExecuteCore();
        //}

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            
            filterContext.Result = new ViewResult() { ViewName = "~/Views/Shared/Error.cshtml" };
            filterContext.RouteData.Values["Message"]= filterContext.Exception.Message;
            base.OnException(filterContext);
        }




    }
}