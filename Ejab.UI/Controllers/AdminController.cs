using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ejab.UI.Controllers
{
    public class AdminController : Controller
    {
        ICustomerService _ICustomerService;
        IRuleService _iRuleService;
        public AdminController(ICustomerService iCustomerService, IRuleService IRuleService)
        {
            _ICustomerService = iCustomerService;
            _iRuleService = IRuleService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()

        { return View(); }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AdminLogin loginModel)
        {
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("loginError", Resources.Global.Loginerror);
            }
            var currentadmin = _ICustomerService.AdminLogIn(loginModel);
            if (currentadmin == null && loginModel.Password != null)
            {
                ViewBag.InvalidData = "InValid Data";
            }
            else if (!_ICustomerService.ChecKEmail(loginModel.Email))
            {
                ViewBag.CheckMail = "This email do not exist";
            }
            if (_ICustomerService.CheckPassword(loginModel.Email, loginModel.Password))
            {
                ViewBag.InvalidData = "InValid Data";
            }
            else if (currentadmin != null)
            {
                var userRules = _iRuleService.ExistedRules(currentadmin.Id);
                if (userRules == null)
                {
                    ViewBag.ErrorExist = "";
                    return View();

                }

                var currentlang = "ar-EG";

                if (Request.Cookies["language"] != null)
                {
                    currentlang = Request.Cookies["language"].Value.ToString();
                }
                var my = new MyPrincipalClone(currentadmin.Id, currentadmin.FirstName, currentadmin.Email, currentadmin.ProfileImgPath, userRules.Select(item => item.Name).ToArray(), currentlang);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(my);

                var authTicket = new FormsAuthenticationTicket(
                   1,
                   currentadmin.FirstName + currentadmin.LastName,
                   DateTime.Now,
                   DateTime.Now.AddDays(90),
                   true,


                   data,
                   "/"
                 );

                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Chart");

            }

            TempData["error"] = "you Have Entered Invalid User Name Of Password";
            return View(loginModel);
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(AdminLogin loginModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var currentadmin = _ICustomerService.AdminLogIn(loginModel);
        //        if (currentadmin == null)
        //        {
        //            TempData["message"] = "you Have Entered Invalid User Name Of Password";
        //        }
        //        if (!_ICustomerService.CheckEmaileExit(loginModel.Email))
        //        {
        //            TempData["EmailExist"] = "This email do not exist";

        //        }
        //        if (_ICustomerService.checkUserdata(loginModel.Email, loginModel.Password))
        //        {
        //            TempData["NotValid"] = "Not Valid data";
        //        }
        //        else if (currentadmin != null)
        //        {
        //            var userRules = _iRuleService.ExistedRules(currentadmin.Id);
        //            List<string> AdminRules = new List<string>();
        //            foreach (var item in userRules)
        //            {
        //                AdminRules.Add(item.Name);
        //            }
        //            var currentlang = Request.Cookies["language"].Value;
        //            var path = Server.MapPath("~/AdminProfileImgs/");
        //            MyPrincipalClone my = new MyPrincipalClone(currentadmin.Id, currentadmin.FirstName, currentadmin.Email, currentadmin.ProfileImgPath, AdminRules.ToArray(), currentlang);
        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(my);

        //            var authTicket = new FormsAuthenticationTicket(
        //               1,
        //               currentadmin.FirstName + currentadmin.LastName,
        //               DateTime.Now,
        //               DateTime.Now.AddDays(90),
        //               true,


        //               data,
        //               "/"
        //             );

        //            encrypt the ticket and add it to a cookie
        //            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
        //            Response.Cookies.Add(cookie);
        //            return RedirectToAction("Index", "Chart");

        //        }
        //    }
        //    ModelState.AddModelError("loginError", Resources.Global.Loginerror);
        //    TempData["error"] = "you Have Entered Invalid User Name Of Password";
        //    return View(loginModel);
        //}
        public ActionResult LogOut()
        {
            //FormsAuthentication.SignOut();
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
            return RedirectToAction("Login", "Admin");
        }
    }
}