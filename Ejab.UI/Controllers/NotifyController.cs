using Ejab.BAL.Services.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Ejab.BAL.ModelViews.Notification;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using Ejab.BAL.Services;
using Ejab.BAL.Common;


namespace Ejab.UI.Controllers
{
    public class NotifyController : BaseController 
    {
        INotificationService _iNotificationService;
        ICustomerService _ICustomerService;
        int pagesize = int.Parse(ConfigurationManager.AppSettings["PageIndex"].ToString());
        public NotifyController(INotificationService INotificationService , ICustomerService ICustomerService)
        {
            _iNotificationService = INotificationService;
            _ICustomerService =ICustomerService;

        }
        [HttpGet]
        // GET: Notify
        public ActionResult Index(string search, int? page = null)
        {
            var model = _iNotificationService.AllNoty(search).ToList().ToPagedList(page ?? 1, pagesize);           

            ViewBag.TotalCount = _iNotificationService.AllNoty(null).ToList().Count();
            return View(model);
        }
        [HttpGet]
        public ActionResult AddNoty()
        {
            return PartialView("/Views/Notify/_AddNoty.cshtml");
        }
        [HttpPost]
        public ActionResult AddNoty(NotificationViewModel model, FormCollection formCollection)
        {
            model.Type = NotificationType.Notification;
            model.BodyArb = model.Body;
            model.TitleArb = model.Title;
            model.Date = DateTime.Now;
           
            bool chkAllUsers = Convert.ToBoolean(formCollection["chkAllUsers"].Split(',')[0]);         
           
            bool chkAllServiceProviders = Convert.ToBoolean(formCollection["chkAllServiceProviders"].Split(',')[0]);
            bool chkAllRequesters = Convert.ToBoolean(formCollection["chkAllRequesters"].Split(',')[0]);
            if (chkAllUsers==true )
            {
               
             
              //  var allUsers = _ICustomerService.AllUsers().ToList();
                PushNotification(model, "/topics/Guests");
                _iNotificationService.AddNoty(model, _User.Id);
               
                return RedirectToAction("Index");
            }
            if (chkAllServiceProviders == true)
            {
               // var allServiceProvider = _ICustomerService.NotyAllServiceProvider().ToList();
               
                PushNotification(model, "/topics/Providers");
                _iNotificationService.AddNoty(model, _User.Id);
               
                return RedirectToAction("Index");
            }
            if (chkAllRequesters == true)
            {
               // var allReuesters = _ICustomerService.NotyAllCustomer().ToList();
               
                PushNotification(model,"/topics/Users");
                _iNotificationService.AddNoty(model, _User.Id);               
                return RedirectToAction("Index");
            }


            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "there is an error");
            }


            return RedirectToAction("Index");
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        // to "/topic/"
        public void PushNotification(NotificationViewModel obj,string Topic)
        {
            try
            {
                var apiKey = "AAAAVnqfvjo:APA91bEvPV3Nm_FgiXzy6ErrB1bSXYDIrlVeS05fxwPkj_BxnQAUH0ni2c-9Ex_BDf2qNH5ou6zqdUuk52x9BXaNpJLGeqPZLk8ORWOYNguYc_PAgOzoiaz1USH7Wd09sl0JjWhE3lZP";
                var SenderId = "371424476730";                
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                var data = new
                {                   
                      
                        senderId = SenderId,
                        notification = new

                        {
                            Date = DateTime.Now,                           
                            body = obj.Body,
                            title = obj.Title

                        },
                        priority = "high",
                    projectId= "naqelat-a5186",
                     to = Topic


                };
                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(data);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.UseDefaultCredentials = true;
                    tRequest.PreAuthenticate = true;
                    tRequest.Credentials = CredentialCache.DefaultCredentials;
                    tRequest.Headers.Add(string.Format("Authorization:key=" + apiKey));
                   // tRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));
                    tRequest.ContentLength = byteArray.Length;
                    tRequest.UseDefaultCredentials = true;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                       
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    Response.Write(sResponseFromServer);
                                    //   Label3.Text = sResponseFromServer;
                                }
                            }
                        }
                    }
                }

            //}
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
       


    }
 



}
}