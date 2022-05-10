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

namespace Ejab.UI.Controllers
{
    public class RegionController : Controller
    {
        IRegionService _iRegionservice;
        //int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        int pagesize = 10;
        public RegionController(IRegionService _RegionService)
        {
            _iRegionservice = _RegionService;
        }
        // GET: Region
        [HttpGet]
        public ActionResult Index(string search, int? page)
        {
            var allregions = _iRegionservice.Regions(search).ToList().ToPagedList(page ?? 1, pagesize);
            ViewBag.totalCount = _iRegionservice.Regions(search).ToList().Count();
            ViewBag.Regions = new SelectList(allregions, "Id", "Name");
            //var cities = _iRegionservice.allRegionswithSearch(1).ToList().ToPagedList(page ?? 1, pagesize);
            return View(allregions);
        }
        //[HttpPost]
        //public ActionResult Index(int parentId, int? page)
        //{
        //    var allregions = _iRegionservice.allRegions().ToList();
        //    ViewBag.Regions = new SelectList(allregions, "Id", "Name");           
        //    var cities = _iRegionservice.allRegionswithSearch(parentId).ToList().ToPagedList(page ?? 1, pagesize);
        //    return View(cities);
        //}
        [HttpGet]
        public ActionResult Get(int id)
        {
            var region = _iRegionservice.getById(id);
            return View(region);
        }
        [HttpGet]
        public ActionResult AddRegion()
        {
            //var allregions = _iRegionservice.allRegions().ToList();
            //ViewBag.Regions = new SelectList(allregions, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddRegion(RegionModelView model)
        {
             
            
            if (ModelState.IsValid)
            {
                if (_iRegionservice.CheckRegionExist(model.Name))
                {
                    ViewBag.message = "this region already exist";
                    return View();
                }
                else
                {
                    var result = _iRegionservice.AddRegion(model, 1);
                    return RedirectToAction("Index");
                }
             

            }

            return View();
        }
        [HttpGet]
        public ActionResult EditRegion(int id)
        {
            var region = _iRegionservice.getById(id);
            //var allregions = _iRegionservice.allRegions().ToList();
            //ViewBag.Regions = new SelectList(allregions, "Id", "Name", region.ParanetId);
          
            return View( region);
        }
        [HttpPost]
        public ActionResult EditRegion(int id, RegionModelView region)
        {
            if (ModelState.IsValid)
            {
                if (_iRegionservice.CheckRegionExist(region.Name))
                {
                    ViewBag.message = "this region already exist";
                    return View();
                }
                var existregion = _iRegionservice.getById(id);
               
                var regionViewModel = _iRegionservice.EditRegion(id, region, 1);
                return RedirectToAction("Index");
            }           

            return View(region);
        }
        [HttpGet]
        public ActionResult DeleteRegion(int id)
        {
            var regionViewModel = _iRegionservice.DeleteRegion(id, 1);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AllCities(int parentid)
        {
            var cities = _iRegionservice.regionByParent(parentid);
            return View(cities);
        }
        //[HttpPost]
        //public ActionResult GetRegions()
        //{
        //    var parents = _iRegionservice.allRegions();
        //    return Json(parents,JsonRequestBehavior.AllowGet);

        //}
    }
}