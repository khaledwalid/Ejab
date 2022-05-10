using Ejab.BAL.Common;
using Ejab.BAL.ModelViews;
using Ejab.DAl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
   public  interface IMessage
    {
        MessageModelView AddMessage(MessageModelView model,int UserId);
        MessageModelView EditMessage(int id,MessageModelView model,int UserId);
        MessageModelView DeleteMessage(int id);
        IEnumerable<MessageModelView> RequestMessage(int requestId);
        IEnumerable<MessageModelView> SendUserMessage(int senderId);
        IEnumerable<MessageModelView> RecivedUserMessage(int ReciverId,int SenderId, int page);
   IEnumerable<   MessageModelView> LastMessageRecived(int ReciverId);
       MessageModelView LastMessageSend( int SenderId);
        MessageModelView GetNexMessage(int id, int ReciverId, int SenderId);
       IEnumerable< object > GetUnReadMsg(int reciverid);
        int Count(int UserId, int reciverId);
        int SentCount(int UserId,int senderId);
        int unReadMessagesCount(int reciverId,int SenderId);
        object  AllLastMessages(int UserId, AllMessagesViewModel messageModel, int? page,HttpRequestMessage Request);
        MessageModelView MessageDetailes(int id);
        IEnumerable<MessageModelView> RecivedMessage(int ReciverId, int page,int pagesize);
        object AllMessages(int UserId, AllMessagesViewModel messageModel, HttpRequestMessage Request, int? page=null);
        object   UserMessages(AllMessagesViewModel model, int currentUser, HttpRequestMessage Request, int? page = null);
        IEnumerable<MessageModelView> All(string search);
        //  void Changestate(IEnumerable<Message> coll,int UserId);
        int MessagesCount(int UserId);
        MessagesFromAdmin AddMessageFromAdmin(MessagesFromAdmin model, int UserId);
        MessagesFromAdmin AddMessageFromAdminToUser(int id,MessagesFromAdmin model, int UserId);

    }
}
