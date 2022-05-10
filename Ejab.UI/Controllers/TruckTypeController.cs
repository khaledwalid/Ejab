using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Configuration;

namespace Ejab.UI.Controllers
{
    public class TruckTypeController : Controller
    {
        ITruckTypeService _iTruckTypeservice;
        ICustomerService _customerService;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public TruckTypeController(ITruckTypeService ITruckTypeService, ICustomerService ICustomerService)
        {
            _iTruckTypeservice = ITruckTypeService;
            //int id = 0;
            //if (Session["CurrentUserId"] != null)
            //{
            //    int currentuserId = Convert.ToInt16(Session["CurrentUserId"].ToString());
            //    if (currentuserId != 0)
            //    {
            //        id = currentuserId;
            //    }
            //}
        }
        [HttpGet ]
        public ActionResult Index(string search, int? page)
        {
            var model = _iTruckTypeservice.AllServiceType(search,page ).ToList().ToPagedList(page ?? 1, pagesize);
        ViewBag.count= _iTruckTypeservice.AllServiceType(search, page).ToList().Count();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AddType()
        {
            return PartialView("~/Views/TruckType/_AddType.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddType(TruckTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _iTruckTypeservice.AddTruckType(model, 1);
             return  RedirectToAction("Index");
              
            }
            return PartialView("~/Views/TruckType/_AddType.cshtml", model);
        }
        [HttpGet]
        public ActionResult EditType(int id)
        {
            var existedType = _iTruckTypeservice.GetTruckTypebyId(id);
            return View(existedType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditType(int id,TruckTypeViewModel model)
        {
            var existedType = _iTruckTypeservice.GetTruckTypebyId(id);
            if (ModelState.IsValid)
            {
                _iTruckTypeservice.EditTruckTypes(id, model,1);
                return RedirectToAction("Index");
            }
            return View(existedType); 
        }
        [HttpGet]
        public ActionResult DeleteType(int id)
        {
            var existedType = _iTruckTypeservice.GetTruckTypebyId(id);
            _iTruckTypeservice.DeleteTruckTypes(id ,1);
            return RedirectToAction("Index");
        }
    }
}