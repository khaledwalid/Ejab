using Ejab.BAL.ModelViews.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.Notification
{
   public  interface INotificationService
    {
        NotificationViewModel AddNoty(NotificationViewModel model,int UserId);
        NotificationViewModel EditNoty(int id,NotificationViewModel model, int UserId);
        NotificationViewModel DeleteNoty(int id, int UserId);
        NotificationViewModel GetNotyById(int id);
        object  NoyiFicationForUser(int UserId, HttpRequestMessage Request, NotificationModel notyModel, int page = 1);
        IEnumerable<NotificationViewModel> AllNoty(string search);

    }
}
