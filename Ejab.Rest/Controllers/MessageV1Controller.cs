using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.BAL.Services;
using Ejab.BAL.UnitOfWork;
using Ejab.DAl;
using Ejab.Rest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ejab.Rest.Controllers
{
    [Authorize ]
    [RoutePrefix("api/V1/Message")]
    public class MessageV1Controller : BaseController
    {
      
           /// <summary>
           /// 
           /// </summary>
           IMessage _message;
        public MessageV1Controller(IMessage message)
        {
            this._message = message;          
        }
      
        [HttpGet]
        [Route("User/{ReciverId}")]
        public ResponseDTO RecivedUserMessage(int ReciverId, int page=0)
        {
            try
            {
              
                var model = _message.RecivedUserMessage(ReciverId,_User.UserId,page );
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

        [HttpGet]
        [Route("RequestMessages/{requestId}")]
        public ResponseDTO RequestMessage(int requestId)
        {
            try
            {
                var model = _message.RequestMessage(requestId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        [HttpGet]
        [Route("MySendMessage")]
        public ResponseDTO MySendMessage()
        {
            try
            {
                var model = _message.SendUserMessage(_User.UserId );
                return new ResponseDTO(model);
            }          
             catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }
        //[HttpGet]
        //[Route("MyRecivedCount")]
        //public ResponseDTO MyRecivedCount()
        //{
        //    try
        //    {
        //        var model = _message.Count(_User.UserId);
        //        return new ResponseDTO(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}
        //[HttpGet]
        //[Route("MySendCount")]
        //public ResponseDTO MySendCount()
        //{
        //    try
        //    {
        //        var model = _message.SentCount (_User.UserId);
        //        return new ResponseDTO(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message, "");
        //    }
        //}

        [HttpPost]
        [Route("AddMessage")]       
        public ResponseDTO Post(MessageModelView model)
        {
            try
            {
                var MessageModel = _message.AddMessage(model, _User.UserId);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpPut]
        [Route("EditMessage/{id}")]
        public ResponseDTO Put(int id, MessageModelView model)
        {
            try
            {
                var MessageModel = _message.EditMessage (id,model, _User.UserId);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }

        [HttpDelete]
        [Route("DeleteMessage/{id}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                var MessageModel = _message.DeleteMessage(id);
                return new ResponseDTO(MessageModel);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
        }


        //[HttpGet]
        //[Route("Notify/{senderId}")]
        //public ResponseDTO Notification(int senderId)
        //{
        //    try
        //    {
        //        var model = _message.LastMessage( _User.UserId, senderId);
        //        return new ResponseDTO(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO(ex.Message,"");
        //    }

        //}

        

       [HttpGet]
        [Route("GetNexMessage/{ReciverId}/{id}")]
        public ResponseDTO GetNexMessage(int ReciverId, int id)
        {
            try
            {
                var model = _message.GetNexMessage(id,ReciverId, _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }

        }

        [HttpGet]
        [Route("GetUnReadMsg")]
        public ResponseDTO GetUnReadMsg()
        {
            try
            {
                var model = _message.GetUnReadMsg(_User.UserId );
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message,"");
            }

        }

        [HttpPost]
        [Route("LastMessage")]
        public ResponseDTO AllLastMessages( AllMessagesViewModel messageModel, int page=1)
        {
            try
            {
                var model = _message.AllLastMessages(_User.UserId , messageModel,page,Request);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        [HttpPost]
        [Route("All")]
        public ResponseDTO AllMessages(AllMessagesViewModel messageModel, int? page =null)
        {
            try
            {
                var model = _message.AllMessages(_User.UserId, messageModel, Request,page);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }
        [HttpPost]
        [Route("UserMessages")]
        public ResponseDTO UserMessages(AllMessagesViewModel messageModel, int? page = null)
        {
            try
            {
                var model = _message.UserMessages(messageModel,_User.UserId, Request, page);
              
                return new ResponseDTO(model);

               
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }
           
        }
        [HttpGet]
        [Route("CountUnReadMess/{senderId}")]
        public ResponseDTO CountUnReadMess(int senderId)
        {
            try
            {
                var model = _message.unReadMessagesCount(_User.UserId, senderId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

        [HttpGet]
        [Route("{id}")]
        public ResponseDTO MessageDetailes(int id)
        {
            try
            {
                var model = _message.MessageDetailes(id);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

        [HttpGet]
        [Route("UserMessages")]
        public ResponseDTO UserMessages(int ReciverId, int page = 0,int PageSize=10)
        {
            try
            {

                var model = _message.RecivedMessage(_User.UserId, page, PageSize);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

        [HttpGet]
        [Route("MyMessagesCount")]
        public ResponseDTO MyMessagesCount()
        {
            try
            {
                var model = _message.MessagesCount( _User.UserId);
                return new ResponseDTO(model);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, "");
            }

        }

    }
}
