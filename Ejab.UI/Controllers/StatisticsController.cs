using Ejab.BAL.ModelViews.Statictics;
using Ejab.BAL.Services.statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class StatisticsController : Controller
    {
        IstatisticsService _istatisticsService;
        public StatisticsController(IstatisticsService IstatisticsService)
        {
            this._istatisticsService = IstatisticsService;
        }
        // GET: Statistics
        public ActionResult Index()
        {
            var model = _istatisticsService.All().ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult AddStatistics()
        {
            //return PartialView ("~/Views/Director/_AddDirector.cshtml");
            return View();
        }
        //[Authorize ]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStatistics(StaticticsViewModel statistics)
        {

            if (!ModelState.IsValid)
            {
                return View(statistics);
            }
            _istatisticsService.AddStatistics(statistics, 1);
            return RedirectToAction("Index");

        }

        [HttpGet]
        //[Authorize]
        public ActionResult EditStatistics(int id)
        {
            var director = _istatisticsService.GetById(id);
            return View(director);
        }
        [HttpPost]
        //[Authorize]
        public ActionResult EditStatistics(int id, StaticticsViewModel stat)
        {

            _istatisticsService.EditStatistics(id, stat, 1);
            return RedirectToAction("Index");
            if (!ModelState.IsValid)
            {
                return View(stat);
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult DeleteStatistics(int id)
        {
            _istatisticsService.DeleteStatistics(id, 1);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TopStatistics()
        {
            var model = _istatisticsService.All().FirstOrDefault() ?? new StaticticsViewModel().Initialize();
            return PartialView("~/Views/Statistics/_TopStatistics.cshtml", model);
        }

    }
}