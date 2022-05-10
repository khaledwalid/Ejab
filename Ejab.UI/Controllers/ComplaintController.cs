using Ejab.BAL.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ejab.UI.Controllers
{
    public class ComplaintController : Controller
    {
        IComplaintService _iComplaintService;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public ComplaintController(IComplaintService IComplaintService)
        {
            _iComplaintService = IComplaintService;
        }
        // GET: Complaint
        public ActionResult Index( int? page)
        {
            var complaints = _iComplaintService.GetAllComplaints().ToList().ToPagedList(page ?? 1, pagesize);
            var totalCount = _iComplaintService.GetAllComplaints().ToList().Count();
            ViewBag.Count = totalCount;
            return View(complaints);
        }
        public ActionResult Deactivate(int id)
        {
            _iComplaintService.MakeSeen(id);
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}