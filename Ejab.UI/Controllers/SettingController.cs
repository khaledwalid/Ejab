using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class SettingController : Controller 
    {
        Isettingcs _Isettingcs;
        public SettingController(Isettingcs Isettingcs)
        {
            _Isettingcs = Isettingcs;
        }
        // GET: Setting
        public ActionResult Index()
        {
            var model = _Isettingcs.GetAll ().ToList();           
            return View(model);
        }

        [HttpGet]
        [Authorize ]
        public ActionResult AddSetting()
        {
            return View(); 
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSetting(SettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _Isettingcs.add (model, 1);
                return RedirectToAction("Index");

            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditSetting(int id)
        {
            var existedType = _Isettingcs.GetById(id);
            return View(existedType);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken()]
        public ActionResult EditSetting(int id, SettingViewModel model)
        {
            var existedType = _Isettingcs.GetById(id);
            if (ModelState.IsValid)
            {
                _Isettingcs.Edit (id, model, 1);
                return RedirectToAction("Index");
            }
            return View(existedType);
        }

        [HttpGet]
        public ActionResult DeleteType(int id)
        {
            var existedType = _Isettingcs.GetById(id);
            _Isettingcs.Delete(id,  1);
            return RedirectToAction("Index");
        }
    }
}