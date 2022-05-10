using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews.Notification;
using Ejab.BAL.Common;
using Ejab.BAL.UnitOfWork;
using System.Web;
using System.Net.Http;
using Ejab.BAL.ModelViews;

namespace Ejab.BAL.Services.Notification
{
    public class NotificationService : INotificationService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        //  int pageSize = PagingConfig.pageSize;
        public NotificationService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public NotificationViewModel AddNoty(NotificationViewModel model, int UserId)
        {
            var entity = factory.Parse(model);

            if (model == null)
            {
                throw new Exception("005");
            }
            //if (model.ReceiverId  == 0)
            //{
            //    throw new Exception("025");
            //}


            entity.Date = DateTime.Now.Date;
            entity.Seen = false;
            entity.SenderId = UserId;
            entity.FlgStatus = 1;
            entity.CreatedBy = UserId;
            entity.CreatedOn = DateTime.Now.Date;
            _uow.Notification.Add(entity);
            _uow.Commit();


            var messageModel = factory.Create(entity);
            //var reciver = _uow.User.GetById(model.ReceiverId  );
            //messageModel.ReciverUser    = factory.Create(reciver);

            return messageModel;


        }

        public NotificationViewModel DeleteNoty(int id, int UserId)
        {
            var noty = _uow.Notification.GetById(id);
            if (noty == null)
            {
                throw new Exception("004");
            }
            if (noty.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            noty.FlgStatus = 0;
            noty.UpdatedBy = UserId;
            noty.UpdatedOn = DateTime.Now.Date;
            _uow.Notification.Update(id, noty);
            _uow.Commit();
            var typeModel = factory.Create(noty);
            return typeModel;
        }

        public NotificationViewModel EditNoty(int id, NotificationViewModel model, int UserId)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
            var noty = _uow.Notification.GetById(id);
            if (noty == null)
            {
                throw new Exception("004");
            }
            noty.Date = model.Date;
            noty.Title = model.Title;
            noty.Body = model.Body;
            noty.Seen = false;
            noty.SenderId = model.SenderId;
            //  noty.DeviceType = model.DeviceType;
            noty.ReceiverId = UserId;
            noty.UpdatedBy = UserId;
            noty.UpdatedOn = DateTime.Now;
            _uow.Notification.Update(id, noty);
            _uow.Commit();
            var typeModel = factory.Create(noty);
            return typeModel;
        }

        public NotificationViewModel GetNotyById(int id)
        {
            var noty = _uow.Notification.GetById(id);
            if (noty == null)
            {

                throw new Exception("006");
            }
            var model = factory.Create(noty);
            return model;
        }

        public object NoyiFicationForUser(int UserId, HttpRequestMessage Request, NotificationModel notyModel, int page = 1)
        {
            NotificationViewModel model = new NotificationViewModel();

            int pagesize = notyModel.PageSize;
            var bodyarb = "";
            var bodyeng = "";
            var titlearb = "";
            var tileeng = "";
            List<string   > res=new List<string  >();
            var rr = new List<NotificationViewModel>();
            var notifications = _uow.Notification.GetAll(x => x.FlgStatus == 1 && x.ReceiverId == UserId).ToList().OrderByDescending(y => y.Id);
            foreach (var item in notifications)
            {
                NotificationViewModel vm = new NotificationViewModel
                {
                    NotyId = item.Id,
                    Date = item.Date,
                    TitleArb = titlearb,
                    TitleEng = tileeng,
                    BodyArb = bodyarb,
                    BodyEng = bodyeng,
                    SenderId = item.SenderId,
                    SenderUser = GetUserById(item.SenderId)
                };
                var bodies = item.Body.Split('-').ToArray();
                if (bodies.Length>1)
                {
                    vm.BodyArb = bodies[0].ToString();
                    vm.BodyEng = bodies[1].ToString();
                    
                  
                }
                var tiltes = item.Title.Split('-').ToArray();
                if (tiltes.Length > 1)
                {
                    vm.TitleArb = tiltes[0].ToString();
                    vm.TitleEng = tiltes[1].ToString();

                }
                rr.Add(vm);
              
            }
            var totalcount = notifications.Count();
            var pagesCount = Math.Ceiling((double)totalcount / pagesize);

            var sortpageValue = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("page");
            int currentPage;
            if (!int.TryParse(sortpageValue, out currentPage))
            {
                currentPage = 0;
            }

            var prevLink = currentPage > 0
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage - 1)
                : "";
            var nextLink = currentPage < pagesCount - 1
                ? Request.RequestUri.AbsoluteUri.Split('?')[0] + '/' + (currentPage + 1)
                : "";
            foreach (var item in notifications)
            {
                var noty = _uow.Notification.GetById(item.Id );
                noty.Seen = true;
                noty.UpdatedBy = UserId;
                noty.UpdatedOn = DateTime.Now;
                _uow.Notification.Update(noty.Id, noty);
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
                    Result = rr.Distinct().ToList().Skip((page - 1) * pagesize).Take(pagesize)
                };


           
        }

        public IEnumerable<NotificationViewModel> AllNoty(string search)
        {
            var allnotifications = _uow.Notification.GetAll(x => x.FlgStatus == 1 ).ToList();
           
            if (search == null)
            {
              
                return allnotifications.OrderByDescending(x=>x.Id).ToList().Select(n => new NotificationViewModel { Date = n.Date, BodyArb  = n.Body , TitleArb = n.Title , NotyId = n.Id, Seen = n.Seen, SenderId = n.SenderId,SenderUser= GetUserById(n.SenderId) });

            }
            return allnotifications.OrderByDescending(x => x.Id).ToList().Where(x => x.SenderUser.FirstName.ToLower().StartsWith(search.ToLower()) || x.ReciverUser.FirstName.ToLower().StartsWith(search.ToLower()) || x.Title.StartsWith(search) || x.Title.Contains(search) ||x.Body.Contains(search) || x.Body.StartsWith(search)).ToList().Select(n => new NotificationViewModel { Date = n.Date, BodyArb = n.Body, TitleArb = n.Title, NotyId = n.Id, Seen = n.Seen, SenderId = n.SenderId , SenderUser = GetUserById(n.SenderId) });
        }
        public UserViewModel GetUserById(int UserId)
        {
                var user = _uow.User.GetAll(x=>x.FlgStatus==1&& x.Id== UserId, null,"").SingleOrDefault();
            if (user !=null )
            {
                var model = factory.Create(user);
                return model;
            }
            return null;
              
        
        }
    }
}
