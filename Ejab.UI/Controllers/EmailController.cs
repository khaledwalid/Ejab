using Ejab.BAL.ModelViews.Email;
using Ejab.BAL.Services.Emailes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class EmailController : Controller
    {
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        private EmaileService _emaileService;
        public EmailController(EmaileService emaileService)
        {
            _emaileService = emaileService;
        }

        // GET: Admin/Email
        public ActionResult Index(int? page)
        {
            var allMailes = _emaileService.AllEmailes().ToList().ToPagedList(page ?? 1, pagesize);
            var totalCount = _emaileService.AllEmailes().ToList().Count();
            ViewBag.Count = totalCount;
            return View(allMailes);
        }

        [HttpGet]
        public ActionResult Subscripe()
        {

            return PartialView("~/Views/Email/_Subscripe.cshtml");
        }

        [HttpPost]
        public ActionResult Subscripe(EmailSubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subscripe = _emaileService.AddEmaile(model, 1);
                ViewBag.Success = " نشكر اشتراكم معنا";
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "please Enter Email To subScripe");
            ViewBag.Faluier = " من فضلك ادخل ايميل للاشتراك";
            return RedirectToAction("Index", "Home");



        }

        [HttpPost]
        //[Route("email")]
        public JsonResult CheckEmailExist([System.Web.Http.FromBody] string email)
        {
            dynamic res = null;
            if (_emaileService.CheckEmail(email))
            {
                res = new
                {
                    message = " تم الاشتراك فى القائمة البريديه من قبل سيصلكم كل جديد لدينا",
                    success = true
                };
            }
            return Json(res);
        }
    }
}