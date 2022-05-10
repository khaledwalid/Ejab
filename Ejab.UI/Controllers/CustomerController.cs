using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using System.Configuration;
using Ejab.DAl.Common;
using Ejab.UI.Models;

namespace Ejab.UI.Controllers
{

    public class CustomerController : BaseController   
    {
        private ICustomerService _ICustomerService;
        IRuleService _iRuleService;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());

        public CustomerController(ICustomerService iCustomerService, IRuleService IRuleService)
        {
            _ICustomerService = iCustomerService;
            _iRuleService = IRuleService;
        }
       
        [HttpGet ]
        public ActionResult AssignRule()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRule(int userId, IList<RuleViewModel> rules, int[] selectedObjects)
        {
           rules = _ICustomerService.AllRules(null).ToList();
            List<RuleViewModel> selectedRules = new List<RuleViewModel>();
            foreach (var item in rules)
            {
                if (selectedObjects.Contains(item.Id))
                {
                    selectedRules.Add(item);
                }
            }
            return View();
        }

      
        // GET: Customer
        [HttpGet ]
        //[Authorize ]
        public ActionResult Index(string search, int? page)
        { 
            var admins = _ICustomerService.AdminUsers(search,page).ToList().ToPagedList(page??1, pagesize);
            ViewBag.totalCunt = _ICustomerService.AdminUsers(null, 0).ToList().Count();
            return View(admins);
        }
        [HttpGet]
        public ActionResult ServiceProviderDetailes(int id)
        {
            var detailes = _ICustomerService.ServiceProviderDetailes(id);
            return View(detailes);
        }
        [HttpGet]
        public ActionResult CustomerDetailes(int id)
        {
            var model = _ICustomerService.CustomerDetailes(id);
            return View(model);
        }
        [HttpGet ]
        [AllowAnonymous]       
        public ActionResult CreateAdmin()
        {
            AdminviewModel adminModel = new AdminviewModel();
            if (Request.Cookies["language"] == null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-EG");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-EG");
            }
          
            var  rules = _ICustomerService.AllRules(null).ToList();
            adminModel.Rules = rules;
            return View(adminModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(AdminviewModel adminModel, HttpPostedFileBase file, IList<RuleViewModel> rules, int[] selectedObjects)
        {
            var allrules = _ICustomerService.AllRules(null).ToList();
            if (_ICustomerService.ChecKEmail(adminModel.Email))
            {
                ViewBag.MailExist = "هذا البريد الالكترونى لديه بالفعل حساب";
                adminModel.Rules = allrules.ToList();
                return View(adminModel);
            }
            if (!ModelState.IsValid)
            {
                adminModel.Rules = allrules;
                return View(adminModel);
            }
            if (selectedObjects == null)
            {
                ViewBag.message = "please Select Admin Rules";
                adminModel.Rules = allrules;
                return View(adminModel);
            }

            if (file != null && file.ContentLength >0)
                {
                    var path = Server.MapPath("~/AdminProfileImgs/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                }
                rules = _ICustomerService.AllRules(null).ToList();
                List<RuleViewModel> selectedRules = new List<RuleViewModel>();
                foreach (var item in rules)
                {
               
                if (selectedObjects != null)
                {
                    if (selectedObjects.Contains(item.Id))
                    {
                        selectedRules.Add(item);

                    }
                }
            }
                adminModel.Rules = rules.ToList();
                _ICustomerService.CreateUserProfile(adminModel, file, _User.Id, rules, selectedObjects);
                return RedirectToAction("Index");
        }
      
        [HttpGet ]
        public ActionResult EditAdmin(int id)
        {
            //if (Request.Cookies["language"]==null)
            //{
            //    HttpCookie langCookie = new HttpCookie("language") { Value = "ar-EG" };
            //    langCookie.Expires = DateTime.Today.AddYears(3);
            //    Response.Cookies.Add(langCookie);
            //}
            //else
            //{
            //    HttpCookie langCookie = new HttpCookie("language") { Value = "en-US" };
            //    langCookie.Expires = DateTime.Today.AddYears(3);
            //    Response.Cookies.Add(langCookie);
            //}
            var admin = _ICustomerService.GetAdmin(id);          
            if (admin !=null)
            {
                var existed = _iRuleService.ExistedRules(id).ToList();                
                if (existed != null)
                {
                    admin.ExistedRules = existed;
                }
            }
            var rules = _ICustomerService.AllRules(null).ToList();
            admin.Rules = rules.ToList();           
            return View(admin);
           
        }
        [HttpPost ]
        [ValidateAntiForgeryToken]     
        public ActionResult EditAdmin(int id, AdminviewModel adminModel, HttpPostedFileBase file , IList<RuleViewModel> rules, int[] selectedObjects)
        {
            var admin = _ICustomerService.GetAdmin(id);
            var allrules = _ICustomerService.AllRules(null).ToList();
            if (!ModelState.IsValid)
            {
                var rulesexisted = _iRuleService.ExistedRules(id).ToList();
              
                if (rulesexisted != null)
                {
                    admin.ExistedRules = rulesexisted;
                }
                
                admin.Rules = allrules.ToList();
                return View(admin);
            }
            if (selectedObjects == null)
            {
                ViewBag.message = "please Select Admin Rules";
               admin.Rules = allrules.ToList();
                return View(admin);
            }
            //if (_ICustomerService.ChecKEmail(adminModel.Email))
            //{
            //    ViewBag.MailExist = "هذا البريد الالكترونى لديه بالفعل حساب";
            //    adminModel.Rules = rules.ToList();
            //    return View(adminModel);
            //}
            if (file != null)
                {
                    var path = Server.MapPath("~/AdminProfileImgs/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);
                }
                rules = _ICustomerService.AllRules(null).ToList();
                List<RuleViewModel> selectedRules = new List<RuleViewModel>();
                if (rules != null)
                {
                    foreach (var item in rules)
                    {
                    if (selectedObjects == null)
                    {
                        ViewBag.message = "please Select Admin Rules";
                        admin.Rules = allrules.ToList();
                        return View(admin);
                    }

                    if (selectedObjects !=null)
                    {
                        if (selectedObjects.Contains(item.Id))
                        {
                            selectedRules.Add(item);

                        }
                    }

                }
                }

                adminModel.Rules = rules.ToList();
                var existed = _iRuleService.ExistedRules(id).ToList();
                //SelectList existedRules = new SelectList(existed, "Id", "Name");
                //ViewBag.existed = existedRules;
                adminModel.ExistedRules = existed.ToList();
                _ICustomerService.EditAdminData(adminModel, id, file, rules, selectedObjects);
                return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteAdmin(int id)
        {
                _ICustomerService.DeleteUser(id);
                return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        { return View(); }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin loginModel)
        {
            if (ModelState.IsValid)
            {
                var currentadmin = _ICustomerService.AdminLogIn(loginModel);
                if (currentadmin == null)
                {
                    TempData["message"] = "you Have Entered Invalid User Name Of Password";
                }
                if (!_ICustomerService.CheckEmaileExit(loginModel.Email))
                {
                    TempData["EmailExist"] = "This email do not exist";

                }
                if (_ICustomerService.checkUserdata(loginModel.Email, loginModel.Password))
                {
                    TempData["NotValid"] = "Not Valid data";
                }
                else if (currentadmin != null)
                {
                    var userRules = _iRuleService.ExistedRules (currentadmin.Id);
                    List<string> AdminRules = new List<string>();
                    foreach (var item in userRules)
                    {
                        AdminRules.Add(item.Name);
                    }
                    var currentlang = Request.Cookies["language"].Value;
                    // var path = Server.MapPath("~/AdminProfileImgs/");
                    MyPrincipalClone my = new MyPrincipalClone(currentadmin.Id, currentadmin.FirstName, currentadmin.Email, currentadmin.ProfileImgPath, AdminRules.ToArray(), currentlang);
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(my);

                    var authTicket = new FormsAuthenticationTicket(
                       1,
                       currentadmin.FirstName + currentadmin.LastName,
                       DateTime.Now,
                       DateTime.Now.AddDays(90),
                       true,


                       data,
                       "/"
                     );

                    //encrypt the ticket and add it to a cookie
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Admin", "Home");

                }
            }
            ModelState.AddModelError("loginError", Resources.Global.Loginerror);
            TempData["error"] = "you Have Entered Invalid User Name Of Password";
            return View(loginModel);
        }


        [HttpGet]
        public ActionResult AdminProfile(int id)
        {
            var currentuserdata = _ICustomerService.GetAdmin(id);
            // var profile = _ICustomerService.EditAdminData(id);
            return View(currentuserdata);
        }

        [HttpGet]
        public ActionResult EditAdminProfile()
        {
                var admin = _ICustomerService.GetAdmin(_User.Id);
                var existed = _iRuleService.ExistedRules(_User.Id ).ToList();
                //SelectList existedRules = new SelectList(existed, "Id", "Name");
                //ViewBag.existed = existedRules;
                if (existed != null)
                {
                    admin.ExistedRules = existed;
                }
                var rules = _ICustomerService.AllRules(null).ToList();
                admin.Rules = rules.ToList();
                return View(admin);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdminProfile( AdminviewModel adminModel, HttpPostedFileBase file, IList<RuleViewModel> rules, int[] selectedObjects)
        {
            var allrules = _ICustomerService.AllRules(null).ToList();
            var admin = _ICustomerService.GetAdmin(_User.Id);
            if (selectedObjects == null)
            {
                ViewBag.message = "please Select Admin Rules";
                admin.Rules = allrules.ToList();
                return View(admin);
            }
            if (!ModelState.IsValid)
            {
                var rulesexisted = _iRuleService.ExistedRules(_User.Id).ToList();

                if (rulesexisted != null)
                {
                    admin.ExistedRules = rulesexisted;
                }
                admin.Rules = allrules.ToList();
                return View(admin);
            }
            //if (_ICustomerService.ChecKEmail(adminModel.Email))
            //{
            //    ViewBag.MailExist = "هذا البريد الالكترونى لديه بالفعل حساب";
            //    adminModel.Rules = rules.ToList();
            //    return View(adminModel);
            //}
            if (file != null)
            {
                var path = Server.MapPath("~/AdminProfileImgs/");
                {
                    Directory.CreateDirectory(path);
                }
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Path.Combine(path, ImageName);

                // save image in folder
                file.SaveAs(physicalPath);
            }
            rules = _ICustomerService.AllRules(null).ToList();
            List<RuleViewModel> selectedRules = new List<RuleViewModel>();
            if (rules != null)
            {
                foreach (var item in rules)
                {
                    if (selectedObjects == null)
                    {
                        ViewBag.message = "please Select Admin Rules";
                    }
                    if (selectedObjects != null)
                    {
                        if (selectedObjects.Contains(item.Id))
                        {
                            selectedRules.Add(item);

                        }
                    }

                }
            }

            adminModel.Rules = rules.ToList();
            var existed = _iRuleService.ExistedRules(_User.Id).ToList();
            //SelectList existedRules = new SelectList(existed, "Id", "Name");
            //ViewBag.existed = existedRules;
            adminModel.ExistedRules = existed.ToList();
            _ICustomerService.EditAdminData(adminModel, _User.Id, file, rules, selectedObjects);
            return RedirectToAction("Index", "Chart");
        }

        [HttpGet ]
        public ActionResult ChangePassword()
        { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassWordModel model)
        {
           
            if (_ICustomerService.CheckPassword(_User.Email,model.OldPassWord ))
            {
                ViewBag.InvalidData = "InValid Data";
            }
            if (ModelState.IsValid )
            { _ICustomerService.ChangePassword(model,_User.Id );
                return RedirectToAction("Index", "Chart");
            }
            return View(model);
        }
        [HttpGet]
       
        public ActionResult AllServiceProviders( string search,int? page)

        {
            var totalCount = _ICustomerService.AllServiceProviders(null).ToList().Count();
            ViewBag.Count = totalCount;
            var allserviceprovider = _ICustomerService.AllServiceProviders(search).ToList().ToPagedList(page??1, pagesize);
            return View(allserviceprovider);
        }
        [HttpGet]
        public ActionResult AllRequsters(string search, int? page)
        {
            var totalCount = _ICustomerService.Allrequesters(null).ToList().Count();
            ViewBag.Count = totalCount;
            var allRequsters = _ICustomerService.Allrequesters(search).ToList().ToPagedList(page ?? 1, pagesize);
            return View( allRequsters);
        }
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult AddServiceProvider()
        {           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult AddServiceProvider(UserViewModel UserModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Server.MapPath("~/ServiceProviders/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                }
                _ICustomerService.CreateServiceProvider(UserModel, file,_User.Id );
                return RedirectToAction("AllServiceProviders");
            }
            ModelState.AddModelError("add", "Sorry error Has Aoocured");
            return View(UserModel);
        }
        [HttpGet ]
       
        public ActionResult EditServiceProvider(int id)
        {
            var serviceprovider = _ICustomerService.GetUserFromadmin(id);
            return View(serviceprovider);
        }
        [HttpPost ]
        [ValidateAntiForgeryToken]
        public ActionResult EditServiceProvider(int id, UserViewModelFromAdmin UserModel, HttpPostedFileBase file)
        {
           
            var serviceprovider = _ICustomerService.GetUserFromadmin(id);
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Server.MapPath("~/ServiceProviders/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);
                }

                _ICustomerService.EditProvider(id, UserModel, file, _User.Id);
                return RedirectToAction("AllServiceProviders");
            }

            return View(UserModel);

        }
        [HttpGet]
      
        public ActionResult DeleteUser(int id)
        {
            ViewBag.isRelated = "";
           
                _ICustomerService.DeleteUser(id);
          
            return RedirectToAction("AllServiceProviders");

        }

        [HttpGet]
        [AllowAnonymous]
     
        public ActionResult AddRequester()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult AddRequester(UserViewModel UserModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Server.MapPath("~/Customers/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                }
                _ICustomerService.CreateRequester(UserModel, file, _User.Id );
                return RedirectToAction("AllServiceProviders");
            }
            ModelState.AddModelError("add", "Sorry error Has Aoocured");
            return View(UserModel);
        }
        [HttpGet]
       
        public ActionResult EditRequester(int id)
        {
            var serviceprovider = _ICustomerService.GetUserFromadmin(id);
            return View(serviceprovider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRequester(int id, UserViewModelFromAdmin UserModel, HttpPostedFileBase file)
        {
            var admin = _ICustomerService.GetUserFromadmin(id);
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Server.MapPath("~/Customers/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);
                }
                _ICustomerService.EditRequrster (id, UserModel, file, _User.Id);
                return RedirectToAction("AllRequsters");
            }
           
            return View(admin);
           
        }

       
        public ActionResult Deactivate(int id,bool stat)
        {

            _ICustomerService.DeActiveUser(id,stat);
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
       
    }
}