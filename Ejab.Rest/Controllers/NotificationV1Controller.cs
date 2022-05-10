using Ejab.BAL.ModelViews.Notification;
using Ejab.BAL.Services.Notification;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ejab.Rest.Controllers
{
    [Authorize]
    [RoutePrefix("api/V1/Noty")]
    public class NotificationV1Controller : BaseController
    {

        INotificationService _noty;
        public NotificationV1Controller(INotificationService noty)
        {
            this._noty = noty;
        }
        [HttpPost]
        [Route("MyNotifications")]
        public ResponseDTO UserNotification(  NotificationModel notyModel, int page = 0)
        {
            try
            {

                var model = _noty.NoyiFicationForUser( _User.UserId,Request,notyModel,page );
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }


        [HttpPost]
        [Route("AddNoty")]
        public ResponseDTO Post(NotificationViewModel model)
        {
            try
            {
                var MessageModel = _noty .AddNoty(model, _User.UserId);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        [HttpPut]
        [Route("EditNoty/{id}")]
        public ResponseDTO Put(int id, NotificationViewModel model)
        {
            try
            {
                var MessageModel = _noty .EditNoty (id, model, _User.UserId);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpDelete]
        [Route("DeleteNoty/{id}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                var MessageModel = _noty.DeleteNoty (id,_User.UserId);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }


    }
}
