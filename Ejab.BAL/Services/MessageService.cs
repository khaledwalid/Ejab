using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;
using Ejab.DAl;
using System.Web;
using Ejab.DAl.Models;
using System.Net.Http;
using Ejab.BAL.ModelViews.Notification;
using Ejab.BAL.Helpers;
using Ejab.BAL.Services.Notification;

namespace Ejab.BAL.Services
{
    public class MessageService : IMessage
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        ICustomerService _customerService;     
        INotificationService _iNotificationService;
        //  int pageSize = PagingConfig.pageSize;
        public MessageService(IUnitOfWork uow , ICustomerService customerservice,  INotificationService INotificationService)
        {
            this._uow = uow;
            factory = new ModelFactory();
            _customerService = customerservice;
            _iNotificationService = INotificationService;
        }
        public MessageModelView AddMessage(MessageModelView model, int UserId)
        {
            var usersToken = new List<string>();
           
            var entity = factory.Parse(model);
            var request = _uow.Request.GetById(model.RequestId);

            if (model == null)
            {
                throw new Exception("005");
            }
            if (model.ReciverId == 0)
            {
                throw new Exception("025");
            }

            if (model.OfferId.HasValue)
            {
                var offer = _uow.Offer.GetById(model.OfferId);
                if (offer.ExpireDate.Value.Date <= DateTime.Now.Date)
                {
                    throw new Exception("75");
                }
            }
            if (model.RequestId != null || model.OfferId != null)
            {
                if (model.OfferId.HasValue)
                {
                    entity.OfferId = model.OfferId.Value;

                }
                else
                {

                    entity.OfferId = model.OfferId.HasValue ? model.OfferId : null;
                }
           
                if (model.RequestId.HasValue)
                {
                    entity.RequestId = model.RequestId.Value;

                }
                else
                {
                    entity.RequestId = model.RequestId.HasValue ? model.RequestId : null;
                }

                if (model.RequestId .HasValue)
                {
                    var myrequest = _uow.Request .GetById(model.RequestId);
                    if (myrequest.ExpireDate<= DateTime.Now.Date)
                    {
                        throw new Exception("75");
                    }
                }

            }

            entity.Date = DateTime.Now;
            entity.Status = false;
            entity.SendingTime = DateTime.Now.ToShortTimeString() ;
            entity.Description = "";
            entity.MessageType = DAl.Common.MessageType.Text;
            entity.SenderId = UserId;
            entity.FlgStatus = 1;
            entity.CreatedBy = UserId;
            entity.CreatedOn = DateTime.Now.Date;
            _uow.Message.Add(entity);
            _uow.Commit();
            if (model.ReciverId !=0)
            {
              //  var id=Nullable<int>
              var  userDevices = _customerService.GetDeviceByTokenAndType(model.ReciverId);
                NotificationViewModel notymodel = new NotificationViewModel();
                foreach (var item in userDevices)
                {
                    if (item != null )
                    {
                        var deviceData = item.Fcmtoken;
                        if (deviceData != null)
                        {
                            if (!usersToken .Contains(item.Fcmtoken))
                            {
                                usersToken.Add(deviceData);
                            }                           
                                               
                        }
                    }
                }
                var user = _uow.User.GetById(model.ReciverId);
                var senderuser = _uow.User.GetById(UserId);
                // notymodel.Body = "تم ارسال رساله الى" + user.FirstName + "" + user.LastName + '-' + "new Message Has Sent To" + ":" + user.FirstName + "" + user.LastName;
                notymodel.Body = model.Description + '-' + model.Description;
                //notymodel.BodyArb = "تم ارسال رساله من" + UserId;
                notymodel.Date = DateTime.Now;
                notymodel.Title = model.Title + '-' + model.Title;
                //  notymodel.Description  = model.Description  + '-' + model.Description ;
                notymodel.Seen = false;
                //notymodel.DeviceToken = deviceToken;
                // usersToken.Add(deviceData);
                notymodel.registration_ids = usersToken;
                notymodel.Type = NotificationType.Message;
                notymodel.BodyArb = "تم ارسال رساله من"    +" "+ senderuser.FirstName + "" + senderuser.LastName;
                notymodel.BodyEng = "new Message Has Sent from"  +" " + ":" + senderuser.FirstName + "" + senderuser.LastName;
                notymodel.TitleArb = "رسالة جديده";
                notymodel.TitleEng = "new Message";
                if (model.ReciverId != 0)
                {
                    notymodel.ReceiverId = model.ReciverId;
                }
                PushNotification push = new PushNotification();
                push.PushNotifications(notymodel, "");

            }


            var messageModel = factory.Create(entity);
            if (model.RequestId != null)
            {
                messageModel.Request = factory.Create(request);
            }
            if (model.OfferId != null)
            {
                var offer = _uow.Offer.GetById(model.OfferId);
                messageModel.Offer = factory.Create(offer);
            }
            var reciver = _uow.User.GetById(model.ReciverId);
            messageModel.Reciver = factory.Create(reciver);
            var sender = _uow.User.GetById(UserId);
            messageModel.Sender = factory.Create(sender);
            return messageModel;

        }

        public MessageModelView DeleteMessage(int id)
        {
            var existedentity = _uow.Message.GetById(id);
            existedentity.FlgStatus = 0;
            _uow.Message.Update(id, existedentity);
            _uow.Commit();
            var messageModel = factory.Create(existedentity);
            return messageModel;
        }

        public MessageModelView EditMessage(int id, MessageModelView model, int UserId)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
            if (model.ReciverId == 0)
            {
                throw new Exception("025");
            }
            // TimeSpan myDateResult = model.SendingTime .TimeOfDay;
            var existedentity = _uow.Message.GetById(id);
            existedentity.Date = DateTime.Now;
            existedentity.SendingTime = model.Date.ToShortTimeString(); // model.SendingTime .Date.Hour+":"+ model.SendingTime.Date.Minute+":"+ model.SendingTime.Date.Second ;
            existedentity.Title = model.Title;
            //  existedentity.Description = model.Description;
            existedentity.Status = false;
            existedentity.SenderId = UserId;
            existedentity.ReciverId = model.ReciverId;
            existedentity.RequestId = model.RequestId;
            existedentity.OfferId = model.OfferId;
            existedentity.FlgStatus = 1;
            existedentity.UpdatedBy = UserId;
            existedentity.UpdatedOn = DateTime.Now.Date;
            _uow.Message.Update(id, existedentity);
            _uow.Commit();
            var request = _uow.Request.GetById(model.RequestId);
            var messageModel = factory.Create(existedentity);
            messageModel.Request = factory.Create(request);
            var reciver = _uow.User.GetById(model.ReciverId);
            messageModel.Reciver = factory.Create(reciver);
            var sender = _uow.User.GetById(UserId);
            messageModel.Sender = factory.Create(sender);
            return messageModel;
        }

        //public IEnumerable<MessageModelView> RecivedUserMessage(int ReciverId,int SenderId)
        //{
        //    var messages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.ReciverId == ReciverId && x.SenderId== SenderId).Select(M => new MessageModelView {Date=M.Date,Title=M.Title,Description=M.Description,Status=M.Status,SenderId=M.SenderId,SenderName=M.Sender. FirstName+""+M.Sender.LastName});
        //    if (messages.Count()==0)
        //    {
        //        throw new Exception("this User Do Not Recive Any Message");
        //    }

        //    return messages;
        //}

        public IEnumerable<MessageModelView> RequestMessage(int requestId)
        {
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.RequestId == requestId).ToList().Select(M => new MessageModelView { MessageId = M.Id, Date = M.Date, Title = M.Title, Status = M.Status, Sender = factory.Create(M.Sender), MessageType = M.MessageType, Description=M.Description });
            if (messages.Count() == 0)
            {
                throw new Exception("026");
            }
            return messages;
        }

        public IEnumerable<MessageModelView> SendUserMessage(int senderId)
        {
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.SenderId == senderId).ToList().Select(M => new MessageModelView { SendingTime = M.SendingTime, Date = M.Date, Status = M.Status, Title = M.Title, Reciver = factory.Create(M.Reciver), Request = factory.Create(M.Request) });
            if (messages.Count() == 0)
            {
                throw new Exception("027");
            }
            return messages;
        }
        public IEnumerable<MessageModelView> LastMessageRecived(int ReciverId)
        {
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.ReciverId == ReciverId && x.Status == false).ToList();


            var query = messages
                      .GroupBy(p => p.SenderId)
                    .Select(M => new MessageModelView { MessageId = M.FirstOrDefault().Id, Date = M.FirstOrDefault().Date, Title = M.FirstOrDefault().Title, Status = M.FirstOrDefault().Status, Sender = factory.Create(M.FirstOrDefault().Sender) }).OrderByDescending(x => x.Date);
            return query.ToList();
        }
        public MessageModelView LastMessageSend(int senderId)
        {
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.SenderId == senderId && x.Status == false).ToList().Select(M => new MessageModelView { MessageId = M.Id, Date = M.Date, Title = M.Title, Status = M.Status, Sender = factory.Create(M.Sender) }).OrderByDescending(x => x.Date).GroupBy(y => y.SenderId);
            return null;
        }
        public MessageModelView GetNexMessage(int id, int ReciverId, int SenderId)
        {
            int NextId = id + 1;
            var message = _uow.Message.GetById(NextId); ;
            var model = factory.Create(message);
            return model;

        }

        //public IEnumerable<MessageModelView> RecivedUserMessage(int ReciverId, int SenderId, PagingConfig<Message> paging)
        //{

        //}

        public IEnumerable<MessageModelView> RecivedUserMessage(int ReciverId, int SenderId, int page)
        {
            int pageSize = 5;
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.ReciverId == ReciverId && x.SenderId == SenderId).Select(M => new MessageModelView { Date = M.Date, Title = M.Title, Status = M.Status, Sender = factory.Create(M.Sender) }).Skip((page - 1) * pageSize).Take(pageSize);
            if (messages.Count() == 0)
            {
                throw new Exception("028");
            }

            return messages;
        }


        public int Count(int UserId, int ReciverId)
        {
            var count = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.Status == false).Where(y => y.SenderId == UserId && y.ReciverId == ReciverId).Count();
            return count;
        }

        public int SentCount(int senderId, int ReciverId)
        {
            return _uow.Message.GetAll(x => x.FlgStatus == 1 && x.Status == false).ToList().Where(y => y.ReciverId == ReciverId && y.SenderId == senderId).Count();

        }

        public IEnumerable<object> GetUnReadMsg(int reciverid)
        {
            var unreadMsgs = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.Status == false && x.ReciverId == reciverid).Select(M => new { SenderId = M.SenderId, SenderName = M.Sender.FirstName + "" + M.Sender.LastName, Title = M.Title, Description = M.Description, Date = M.Date });
            if (unreadMsgs == null)
            {
                throw new Exception("043");

            }
            return unreadMsgs;
        }
        

        public int unReadMessagesCount(int reciverId, int SenderId)
        {
            var count = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.Status == false, null, "").Where(y => y.ReciverId == reciverId && y.SenderId == SenderId).Count();
            return count;
        }

        public object AllLastMessages(int UserId, AllMessagesViewModel messageModel, int? page, HttpRequestMessage Request)
        {           
            int pageSize = messageModel.PageSize;
            AllMessagesViewModel model = new AllMessagesViewModel();
            List<MessageModelView> output = new List<MessageModelView>();
            var usrsRepo = _uow.User.GetAll(u => u.FlgStatus == 1);
            var msgsRepo = _uow.Message.GetAll(u => u.FlgStatus == 1).OrderByDescending(x=>x.Id);
            var usrs = (from u in usrsRepo                           
                        join mr in msgsRepo on u.Id equals mr.ReciverId
                        group u by u.Id into rslt
                        select rslt.Key).Distinct().OrderBy(s => s);
            var SenderUsrs = (from u in usrsRepo
                              join ms in msgsRepo on u.Id equals ms.SenderId
                              group u by u.Id into rslt
                              select rslt.Key).Distinct().OrderBy(s => s);
            var AllUsers = new List<int>();
            AllUsers.Clear();
            AllUsers.AddRange(usrs);
            AllUsers.AddRange(SenderUsrs);
            AllUsers = AllUsers.Distinct().ToList();
            model.Count = AllUsers.Count();
            var pagesCount = Math.Ceiling((double)model.Count / pageSize);
            var usrsPage = AllUsers;

            foreach (var usrId in usrsPage)
            {
                if (usrId != UserId)//Select from sent
                {
                    var lastMsg = msgsRepo.Where(m => (m.ReciverId == UserId || m.SenderId == UserId) && (m.ReciverId == usrId || m.SenderId == usrId))
                        .OrderByDescending(m => m.Date).FirstOrDefault();
                    if (lastMsg != null)
                        output.Add(new MessageModelView()
                        {
                            MessageId = lastMsg.Id,
                            Title = lastMsg.Title,
                            Date = lastMsg.Date,
                            SendingTime= lastMsg.SendingTime,
                            SenderId = lastMsg.SenderId,
                            UserType=lastMsg.Sender.CustomerType,
                            ReciverId = lastMsg.ReciverId,
                            MessageType = lastMsg.MessageType,
                            OfferId = lastMsg.OfferId,
                            RequestId = lastMsg.RequestId,
                            Sender = factory.Create(lastMsg.Sender),
                            Reciver  = factory.Create(lastMsg.Reciver),
                            Count=SentCount(usrId,UserId  )
                            
                        });
                }
            }

            var pageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(pageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";

            return
                    new
                    {
                        totalCount = output.Count,
                        pagesNumber = pagesCount,
                        CurrentPage = currentPage,
                        PrevPage = prevLink,
                        NextPage = nextLink,
                        Result = output.OrderByDescending(x=>x.Date).OrderByDescending(x => x.SendingTime).Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToList()
                    };
        }

        public MessageModelView MessageDetailes(int id)
        {
            var message = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.Id == id, null, "").ToList().Select(m => new MessageModelView { MessageId = m.Id, Title = m.Title, Date = m.Date, SendingTime = m.SendingTime, Sender = factory.Create(m.Sender), Status = m.Status }).FirstOrDefault();
            return message;

        }

        public IEnumerable<MessageModelView> RecivedMessage(int ReciverId, int page, int pagesize = 10)
        {
            var allMessages = _uow.Message.GetAll(x => x.FlgStatus == 1 && x.ReciverId == ReciverId, null, "")
         .Select(m => new MessageModelView { Title = m.Title, Date = m.Date, SendingTime = m.SendingTime, Sender = factory.Create(m.Sender) })
       .Skip(pagesize * (page - 1)).Take(pagesize);
            return allMessages.ToList();
        }

        public object AllMessages(int CurrentUserId, AllMessagesViewModel messageModel, HttpRequestMessage Request, int? page = null)
        {
            int pageSize = messageModel.PageSize;
            AllMessagesViewModel model = new AllMessagesViewModel();
          
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1,null,"").Where(x => (x.SenderId == CurrentUserId && x.ReciverId == messageModel.UserId && x.Status == false) || (x.ReciverId == CurrentUserId && x.SenderId == messageModel.UserId && x.Status == false)).ToList();
            var CurrentUser = _uow.User.GetById(CurrentUserId);
            var output = messages
                    .Select(m => new MessageModelView { Reciver = factory.Create(m.Reciver), MessageId = m.Id, MessageType = m.MessageType, Title = m.Title, Status = m.Status, RequestId = m.RequestId.HasValue ? m.RequestId.Value : 0, OfferId = m.OfferId.HasValue ? m.OfferId.Value : 0, Sender = factory.Create(CurrentUser) }).ToList().Skip((page ?? 1 - 1) * pageSize).Take(pageSize);
            foreach (var item in messages.ToList())
            {
                item.Status = true;
                item.UpdatedBy = CurrentUserId;
                item.UpdatedOn = DateTime.Now;
                _uow.Message.Update(item.Id, item);
                _uow.Commit();
            }
            model.Count = messages.Count();
            var totalcount = messages.Count();

            var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            var pageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(pageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";

            return
                new
                {
                    totalCount = totalcount,
                    pagesNumber = pagesCount,
                    CurrentPage = currentPage,
                    PrevPage = prevLink,
                    NextPage = nextLink,
                    Result = output.ToList()
                };
        }

        public object UserMessages(AllMessagesViewModel messageModel, int currentUser, HttpRequestMessage Request, int? page = null)
        {
            int pageSize = messageModel.PageSize;
         
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1).ToList();           
            List<Message> allUsermessages = new List<Message>();
           var  recivedmessages = messages.Where(x => (x.ReciverId == currentUser ) );
         var   Sendmessages = messages.Where(x => (x.SenderId == currentUser ));
            allUsermessages.AddRange(recivedmessages);
            allUsermessages.AddRange(Sendmessages);
            allUsermessages = allUsermessages.Where(x => x.ReciverId == currentUser && x.SenderId == messageModel.UserId || x.ReciverId == messageModel.UserId && x.SenderId == currentUser).OrderByDescending(x => x.Id).ThenByDescending(x => x.Date).ToList();
            
            if (messageModel.RequestId != null)
            {
                messages = messages.Where(x => x.SenderId == currentUser && x.ReciverId == messageModel.UserId && x.RequestId == messageModel.RequestId || x.ReciverId == currentUser && x.SenderId == messageModel.UserId && x.RequestId == messageModel.RequestId).OrderByDescending(x => x.Id).ThenByDescending(x => x.Date).ToList();
                allUsermessages.Clear();
                allUsermessages.AddRange(messages);

            }
            if (messageModel.OfferId != null)
            {
                allUsermessages.Clear();
                messages = messages.Where(x => x.SenderId == currentUser && x.ReciverId == messageModel.UserId || x.SenderId == messageModel.UserId && x.OfferId == messageModel.OfferId).OrderByDescending(x => x.Id).ToList();
                allUsermessages.AddRange(messages);

            }
            var output = allUsermessages.ToList().Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToList()
                      .Select(m => new MessageModelView { Sender = factory.Create(m.Sender), Reciver = factory.Create(m.Reciver), MessageId = m.Id, MessageType = m.MessageType, Title = m.Title, Status = m.Status, RequestId = m.RequestId.HasValue ? m.RequestId.Value : 0, OfferId = m.OfferId.HasValue ? m.OfferId.Value : 0,Date=m.Date,SendingTime=m.SendingTime  }).ToList();

            var totalcount = allUsermessages.Count();

            var pagesCount = Math.Ceiling((double)totalcount / pageSize);

            var pageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(pageValue, out currentPage))
            {
                currentPage = 0;
            }
            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";
            foreach (var item in messages.ToList())
            {
                item.Status = true;
                item.UpdatedBy = currentUser;
                item.UpdatedOn = DateTime.Now;
                _uow.Message.Update(item.Id, item);
                _uow.Commit();
            }
            return
                new
                {
                    totalCount = totalcount,
                    pagesNumber = pagesCount,
                    CurrentPage = currentPage,
                    PrevPage = prevLink,
                    NextPage = nextLink,
                    Result = output.ToList()

                };
        }

        public IEnumerable<MessageModelView> All(string search)
        {
          
            int requestNo = 0;
            int.TryParse(search, out requestNo);
            int senderId = 0;
            int.TryParse(search, out senderId);
           
            int reciverId = 0;
            int.TryParse(search, out reciverId);
            DateTime date;
            DateTime.TryParse(search,out date);
            int offerNo = 0;
            int.TryParse(search, out offerNo);
            var messages = _uow.Message.GetAll(x => x.FlgStatus == 1).OrderByDescending(x=>x.Id);
            if (search == "" || search==null)
            {

                return messages.ToList().Select(m => new MessageModelView
                {
                    Date = m.Date,
                    SendingTime = m.SendingTime,
                    Reciver = factory.Create(m.Reciver),
                    Sender = factory.Create(m.Sender),
                    MessageId = m.Id,
                    MessageType = m.MessageType,
                    Title = m.Title,
                    Status = m.Status,
                    //RequestId = m.Request != null ? m.Request.RequestNumber : 0,
                    //OfferId = m.Offer != null ? int.Parse(m.Offer.OfferNumber.ToString()) : 0,
                    RequestId = m.RequestId.HasValue ? m.RequestId : 0,
                    OfferId = m.OfferId.HasValue ? m.OfferId : 0,

                    Description = m.Description
                });
            }
           
            return messages.ToList().Where(x => (x.Reciver.FirstName.Trim()+" "+ x.Reciver.LastName.Trim()).StartsWith(search) || (x.Sender .FirstName.Trim() + " "+ x.Sender.LastName.Trim()).StartsWith(search) 
            || (x.Sender.FirstName.Trim() + " "+ x.Sender.LastName.Trim()).Contains(search)|| (x.Reciver.FirstName.Trim() + " "+x.Reciver.LastName.Trim()).Contains(search)
            || (x.Date.Year.Equals(date.Year) && x.Date.Month.Equals(date.Month) && x.Date.Day.Equals(date.Day))
         || x.RequestId.Equals(requestNo ) || x.RequestId.Equals(null) || 
         x.OfferId.Equals(null) ||  x.OfferId .Equals( offerNo) || x.Title.Contains(search) || 
         x.Title .StartsWith(search) || x.Description.Contains(search) || x.Description.StartsWith(search) ||
         x.SenderId .Equals( senderId) || x.ReciverId .Equals( reciverId) ).Select(m => new MessageModelView
            {
                Date = m.Date,
                SendingTime = m.SendingTime,
                Reciver = factory.Create(m.Reciver),
                Sender = factory.Create(m.Sender),
                MessageId = m.Id,
                MessageType = m.MessageType,
                Title = m.Title,
                Status = m.Status,
               //RequestId = m.Request!=null? m.Request.RequestNumber : 0,
               //OfferId = m.Offer !=null  ? int.Parse(m.Offer .OfferNumber.ToString()): 0,
               RequestId = m.RequestId.HasValue ?m.RequestId :0,
               OfferId = m.OfferId.HasValue?m.OfferId :0,



               Description = m.Description
            });

          
            
        }

        public void Changestate(IEnumerable<Message> coll, int UserId)
        {
            foreach (var item in coll.ToList())
            {
                item.Status = true;
                item.UpdatedBy = UserId;
                item.UpdatedOn = DateTime.Now;
                _uow.Message.Update(item.Id, item);
                _uow.Commit();
            }
        }

        public int MessagesCount(int UserId)
        {
          return   _uow.Message.GetAll(x => x.FlgStatus == 1&& x.ReciverId== UserId&& x.Status==false, null, "").Count();
        }

        public MessagesFromAdmin AddMessageFromAdmin(MessagesFromAdmin model, int UserId)
        {
            var usersToken = new List<string>();

            var entity = factory.Parse(model);

            entity.OfferId = null;
            entity.RequestId = null;
            entity.Date = DateTime.Now;
            entity.Description = model.MessageTitle;
            entity.Status = false;
            entity.SendingTime = DateTime.Now.ToShortTimeString();
            entity.Description = model.Description;
            entity.MessageType = DAl.Common.MessageType.Text;
            entity.SenderId = UserId;
            entity.FlgStatus = 1;
            entity.CreatedBy = UserId;
            entity.CreatedOn = DateTime.Now.Date;
            _uow.Message.Add(entity);
            _uow.Commit();
            if (model.ReciverId != 0)
            {
                //  var id=Nullable<int>
                var devices = _customerService.GetDeviceByTokenAndType(model.ReciverId);
                NotificationViewModel notymodel = new NotificationViewModel();
                if (devices.Count()>0)
                {
                    var user = _uow.User.GetById(model.ReciverId);
                    // notymodel.Body = "تم ارسال رساله الى" + user.FirstName + "" + user.LastName + '-' + "new Message Has Sent from" + ":" + user.FirstName + "" + user.LastName;
                    notymodel.BodyArb = "تم ارسال رساله من" + user.FirstName + "" + user.LastName;
                    notymodel.BodyEng = "new Message Has Sent from" + ":" + user.FirstName + "" + user.LastName;
                    foreach (var item in devices)
                    {
                        if (item != null)
                        {

                            usersToken.Add(item.Fcmtoken);
                            if (model.ReciverId != 0)
                                {
                                    notymodel.ReceiverId = model.ReciverId;
                                }
                            }
                    }
                }
                notymodel.Body = model.Description + '-' + model.Description;
                notymodel.Date = DateTime.Now;
                notymodel.Title = model.MessageTitle + '-' + model.MessageTitle;
                notymodel.Description = model.Description;
                notymodel.Seen = false;               
                notymodel.Type = NotificationType.Message;              
                notymodel.TitleArb = "رسالة جديده";
                notymodel.TitleEng = "new Message";
                notymodel.registration_ids = usersToken;
                PushNotification push = new PushNotification();
                push.PushNotifications(notymodel, "");
            }
            var messageModel = factory.CreateFromAdmin(entity);
            return messageModel;

        }

        public MessagesFromAdmin AddMessageFromAdminToUser(int id,MessagesFromAdmin model, int UserId)
        {
            var usersToken = new List<string>();

            var entity = factory.Parse(model);
            entity.ReciverId = id;
            entity.OfferId = null;
            entity.RequestId = null;
            entity.Date = DateTime.Now;
            entity.Description = model.MessageTitle;
            entity.Status = false;
            entity.SendingTime = DateTime.Now.ToShortTimeString();
            entity.Description = model.Description;
            entity.MessageType = DAl.Common.MessageType.Text;
            entity.SenderId = UserId;
            entity.FlgStatus = 1;
            entity.CreatedBy = UserId;
            entity.CreatedOn = DateTime.Now.Date;
            _uow.Message.Add(entity);
            _uow.Commit();
            if (model.ReciverId != 0)
            {
                //  var id=Nullable<int>
                var devices = _customerService.GetDeviceByTokenAndType(model.ReciverId);
                NotificationViewModel notymodel = new NotificationViewModel();
                if (devices.Count() > 0)
                {
                    var user = _uow.User.GetById(model.ReciverId);
                    // notymodel.Body = "تم ارسال رساله الى" + user.FirstName + "" + user.LastName + '-' + "new Message Has Sent from" + ":" + user.FirstName + "" + user.LastName;
                    notymodel.BodyArb = "تم ارسال رساله من" + user.FirstName + "" + user.LastName;
                    notymodel.BodyEng = "new Message Has Sent from" + ":" + user.FirstName + "" + user.LastName;
                    foreach (var item in devices)
                    {
                        if (item != null)
                        {

                            usersToken.Add(item.Fcmtoken);
                            if (model.ReciverId != 0)
                            {
                                notymodel.ReceiverId = model.ReciverId;
                            }
                        }
                    }
                }
                notymodel.Body = model.Description + '-' + model.Description;
                notymodel.Date = DateTime.Now;
                notymodel.Title = model.MessageTitle + '-' + model.MessageTitle;
                notymodel.Description = model.Description;
                notymodel.Seen = false;
                notymodel.Type = NotificationType.Message;
                notymodel.TitleArb = "رسالة جديده";
                notymodel.TitleEng = "new Message";
                notymodel.registration_ids = usersToken;
                PushNotification push = new PushNotification();
                push.PushNotifications(notymodel, "");
            }
            var messageModel = factory.CreateFromAdmin(entity);
            return messageModel;

        }
    }
}
