using Ejab.BAL.ModelViews.AboutUs;
using Ejab.BAL.Services.AboutUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class AboutUsController : Controller 
    {
        IAboutUs _iAboutUs;
        public double latitude;
        public double longitude;
        public AboutUsController(IAboutUs IAboutUs)
        {
            _iAboutUs = IAboutUs;
        }
        public ActionResult Index()
        {
            var aboutUs = _iAboutUs.GetAll();
            return View(aboutUs);
        }
        [HttpGet]
        public ActionResult AddAboutUs()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAboutUs(AboutUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error Has Occured");

            }
            latitude = model.latitude;
            longitude = model.Longitude;
            ViewBag.Latitude = latitude;
            ViewBag.Longitude = longitude;
            _iAboutUs.AddAboutUs(model,1);
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public ActionResult EditAboutUs(int id)
        {
            var aboutUs = _iAboutUs.Get(id);
            return View(aboutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAboutUs(int id,AboutUsViewModel model)
        {
            var aboutUs = _iAboutUs.Get(id);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _iAboutUs.EditAboutUs (id, model, 1);
            return RedirectToAction("Index");

        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteAboutUs(int id)
        {
            _iAboutUs.DeleteAboutUs(id,1);
            return RedirectToAction("Index");
        }
    }
}