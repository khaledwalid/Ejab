using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ejab.UI.Controllers
{
    public class HomeController : BaseController   
    {
       // private ICustomerService _ICustomerService;
        public HomeController()
        {
        // this._ICustomerService = iCustomerService;
          
        }      
        public ActionResult Index()
        {
          
            ViewBag.tiwitter = ConfigurationManager.AppSettings["tiwitter"];
            ViewBag.pinteres = ConfigurationManager.AppSettings["pinteres"];
            ViewBag.google = ConfigurationManager.AppSettings["google"];
            ViewBag.facebook = ConfigurationManager.AppSettings["facebook"];
            ViewBag.instagram = ConfigurationManager.AppSettings["instagram"];
            return View();
        }

        public ActionResult Admin()
        {
            ViewBag.Message = "welcom in naqlate.";        
            return View();
        }
        
    
        //[HttpPost]
        public ActionResult SetLang(string lang, string currentURL)
        {
            if (lang == "english")
            {
                HttpCookie langCookie = new HttpCookie("language") { Value = "en-US" };
                langCookie.Expires = DateTime.Today.AddYears(3);
                Response.Cookies.Add(langCookie);

            }
            else if (lang == "arabic")
            {
               
                HttpCookie langCookie = new HttpCookie("language") { Value = "ar-EG" };
                langCookie.Expires = DateTime.Today.AddYears(3);
                Response.Cookies.Add(langCookie);
            }
            return Redirect(currentURL);//RedirectToAction("Admin");
        }

   

    }
}