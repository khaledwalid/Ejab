using Ejab.BAL.ModelViews.AboutApplication;
using Ejab.BAL.Services.AboutApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class AboutAppController : Controller  
    {
        IAboutAppService _iAboutApp;
        public AboutAppController(IAboutAppService IAboutApp)
        {
            _iAboutApp = IAboutApp;
        }
        public ActionResult Index()
        {
            var aboutApp = _iAboutApp.GetAll();
            return View(aboutApp);
        }
        [HttpGet]
        public ActionResult AddAboutApp()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAboutApp(AboutAppViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error Has Occured");

            }
            _iAboutApp.AddAboutApp(model,1);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult EditAboutApp(int id)
        {
            var aboutUs = _iAboutApp.Get(id);
            return View(aboutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAboutApp(int id, AboutAppViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError(string.Empty, "Error Has Occured");
                return View(model);
            }
            _iAboutApp.EditAboutApp(id, model, 1);
            return RedirectToAction("Index");

        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteAboutApp(int id)
        {
            _iAboutApp.DeleteAboutApp(id, 1);
            return RedirectToAction("Index");
        }
    }
}