using Ejab.BAL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Ejab.UI.Controllers
{
    public class RequestController : Controller
    {
        IRequestService _iRequestService;
        int  pagesize =int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public RequestController(IRequestService _IRequestService)
        {
            _iRequestService = _IRequestService;
        }
     
        // GET: Request
        [HttpGet]
        public ActionResult Index(string search, int?page)
        {
            ViewBag.TotalCount = _iRequestService.AllRequests(null).ToList().Count();
            if (search==null )
            {
              return  View( _iRequestService.AllRequests(null).ToList().ToPagedList(page ?? 1, pagesize));
            }
          var   model = _iRequestService.AllRequests(search).ToList().ToPagedList(page??1, pagesize);
          
            return View(model);
        }
        [HttpGet]
        public ActionResult RequestDetailes(int id)
        {
            var model = _iRequestService.GetallDataforRequest(id);
            return View(model);
        }
        //[HttpPost]
        //public ActionResult RequestDetailes(int id)
        //{
        //    var model = _iRequestService.GetallDataforRequest(id);
        //    return View(model);
        //}

        public ActionResult Deactivate(int id, bool stat)
        {
            _iRequestService.DeActiveRequest(id, stat);
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}