using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using Ejab.BAL.ModelViews;
using System.Configuration;

namespace Ejab.UI.Controllers
{
    public class OfferController : Controller
    {
        IOfferService _iOfferService;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public OfferController(IOfferService _IOfferService)
        {
            _iOfferService = _IOfferService;
        }
        // GET: Offer
        //[Authorize ]
        [HttpGet ]
        public ActionResult Index(string search, int? page)
        {
            var model = _iOfferService.SearchAdmin(search).ToList().ToPagedList(page?? 1, pagesize);
            var totalCount = _iOfferService.SearchAdmin(null).ToList().Count();
            ViewBag.Count = totalCount;
            return View(model);
        }
        public ActionResult OfferDetailes(int id=0)
        {          
            var offerModel = _iOfferService.GetOfferData(id);
         
            return View(offerModel); 
        }
    
        public ActionResult   Deactivate(int id,bool stat)
        {
            _iOfferService.DeActiveOffer(id, stat);
            return RedirectToAction("Index");      
        }

    }
}