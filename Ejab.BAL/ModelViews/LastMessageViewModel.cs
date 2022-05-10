using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class LastMessageViewModel
    {
        public   MessageModelView lastmessage { get; set; }
        public int RequestId { get; set; }
        public UserViewModel  Sender { get; set; }
        public UserViewModel Reciver { get; set; }
        public RequestModelView Request { get; set; } // FK_Message_Request
        public int Count { get; set; }
    }
}
