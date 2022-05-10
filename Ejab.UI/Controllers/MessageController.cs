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
    public class MessageController : BaseController
    {
        IRequestService _iRequestService;
        IOfferService _iOfferService;
        ICustomerService _iCustomerService;
        IMessage _imessage;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public MessageController(IMessage IMessage, IRequestService IRequestService, IOfferService IOfferService, ICustomerService ICustomerService)
        {
            _imessage = IMessage;
            _iRequestService = IRequestService;
            _iOfferService = IOfferService;
            _iCustomerService = ICustomerService;
        }
        // GET: Message
        public ActionResult Index(string search, int? page = null)
        {
             var model=   _imessage.All(search).ToPagedList(page??1, pagesize);
            ViewBag.TotalCount = _imessage.All(null).ToList().Count();
            return View(model);
        }
        [HttpGet]
        public JsonResult ReturnUsers()
        {

            var jsonData = _iCustomerService.AllUsers().ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ReturnOffers()
        {

            var jsonData = _iOfferService.Search(null).ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ReturnRequsts()
        {

            var jsonData = _iRequestService.GetRequests().ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet ]
        //[Authorize]
        public ActionResult AddMessage()
        {
            var allUsers = _iCustomerService.AllUsers().ToList();
            ViewBag.AllUsers = new SelectList(allUsers, "Id", "FullName");
            return View();
        }

        [HttpPost]
      //  [Authorize]
        public ActionResult AddMessage(MessagesFromAdmin model)
        {
            var allUsers = _iCustomerService.AllUsers().ToList();
            ViewBag.AllUsers = new SelectList(allUsers, "Id", "FullName");
            if (ModelState.IsValid)
            {
                _imessage.AddMessageFromAdmin(model,_User.Id);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        //[Authorize]
        public ActionResult AddMessageToUser(int id)
        {
          
            return View();
        }

        [HttpPost]
        //  [Authorize]
        public ActionResult AddMessageToUser(int id,MessagesFromAdmin model)
        {
           
            if (ModelState.IsValid)
            {
                model.ReciverId = id;
                _imessage.AddMessageFromAdminToUser(id,model, _User.Id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}