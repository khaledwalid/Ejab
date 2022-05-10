using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using Ejab.BAL.ModelViews;
using System.IO;

namespace Ejab.UI.Controllers
{
    public class TruckController : Controller
    {
        ITruckService _itruckService;
        ITruckTypeService _iTruckTypeservice;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public TruckController(ITruckService ITruckService, ITruckTypeService ITruckTypeService)
        {
            _itruckService = ITruckService;
            _iTruckTypeservice = ITruckTypeService;
        }
        // GET: Truck
        //[HttpGet]
        //public ActionResult Index(int typeid, string search, int? page)
        //{
        //    var model = _itruckService.GetallTrucks(typeid, search, page).ToList().ToPagedList(page ?? 1, pagesize);
        //    return View();
        //}

        [HttpPost]
        public JsonResult GettrucksUndertype(int typeid)
        {
            var alltrucks = _itruckService.TrucksByType(typeid);
            SelectList trucksList = new SelectList(alltrucks, "Id", "Name");
            ViewBag.trucks = trucksList;
            return Json(new SelectList(trucksList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        [HttpGet]
        public ActionResult Index()
        {
           TrucksViewModel mymodel = new TrucksViewModel();
            var parentTrucks = _itruckService.GetallTrucks().Where(x => x.ParenetId == null).ToList();

            var alltypes = _iTruckTypeservice.AllServiceType()
                .Select(tt => new TrucksViewModel()
                {
                    Name = tt.Name,
                    NameArb = tt.NameArb,
                    Id = tt.TypeId,
                    TruckTypeId = tt.TypeId,
                  ParenetId = -1
                }).ToList();

            foreach (var item in parentTrucks)
            {
                var childTrucks = _itruckService.TrucksByParent(item.Id);
                item.ChildModel = childTrucks;
            }

            foreach (var item in alltypes)
            {
                var trucks = parentTrucks.Where(p => p.truckType.TypeId == item.Id);
                item.ChildModel = trucks;
            }


            ViewBag.trucksJson = Json(alltypes, JsonRequestBehavior.AllowGet);
            //  return Json(parentTrucks, JsonRequestBehavior.AllowGet);

            return View();

        }
        [HttpGet]
        public ActionResult AddTruck(int Id, int typeId,int parentId)
        {
            if (parentId == -1)
            {
                parentId = 0;
            }
            //ViewBag.Id = id;
            ViewBag.parentId = parentId;
            ViewBag.typeId = typeId;
            //ViewBag.Id = id;
            TrucksViewModel mm = new TrucksViewModel();
            mm.Id = Id;
            mm.ParenetId = parentId;
            mm.TruckTypeId = typeId;
            return PartialView("~/Views/Truck/_AddTruck.cshtml", mm);
            //return View();
        }
        [HttpPost]
        [ValidateInputAttribute(false)]
        public ActionResult AddTruck(TrucksViewModel trucksModel, HttpPostedFileBase file,int Id ,int parentId, int typeId)
        {
            if (!ModelState.IsValid )
            {
                return PartialView("~/Views/Truck/_AddTruck.cshtml", trucksModel);
            }
            if (parentId ==-1)
            {
                parentId = 0 ;
            }
                if (file != null)
                {
                    var path = Server.MapPath("~/TrucksIcons/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    trucksModel.TruckImagePath = ImageName;
                }
                _itruckService.AddTruckFromAdmin(Id, typeId, trucksModel, 1, file);
                return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditTruck(int id)
        {
            var model = _itruckService.getById(id);
            return PartialView("~/Views/Truck/_EditTruck.cshtml", model);
        }
        [HttpPost]
        public ActionResult EditTruck(int id, TrucksViewModel trucksModel, HttpPostedFileBase file)
        {
          
            var model = _itruckService.getById(id);
            //if (ModelState.IsValid)
            //{
                if (file != null)
                {
                    var path = Server.MapPath("~/TrucksIcons/");
                    {
                        Directory.CreateDirectory(path);
                    }
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(path, ImageName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    trucksModel.TruckImagePath = ImageName;
                }
                _itruckService.EditTruckFromAdmin(id, trucksModel, 1, file);
                return RedirectToAction("Index");
            //}
            //return PartialView("~/Views/Truck/_EditTruck.cshtml", model);
        }

        [HttpGet]
        public ActionResult GettruckById(int id)
        {
            var truck = _itruckService.getById(id);
            return Json(truck, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteTruck(int id)
        {
            _itruckService.DeleteTruck(id, 1);
            return RedirectToAction("Index");
        }
    }
}