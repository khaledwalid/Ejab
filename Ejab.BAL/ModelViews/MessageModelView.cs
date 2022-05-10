using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class MessageModelView
    {
        public int MessageId { get; set; }
        public System.DateTime Date { get; set; } // Date
        public string  SendingTime { get; set; } // Date
        [Required(ErrorMessage = " Message Title Is Required")]
        public string Title { get; set; } // Title (length: 50)
        //[Required(ErrorMessage = " Message Description Is Required")]
        //public string Description { get; set; } // Description (length: 500)
        public bool? Status { get; set; } // Status
        public MessageType MessageType { get; set; }
          public int? RequestId { get; set; }

        public  RequestModelView Request { get; set; } // FK_Message_Request
        public int? OfferId { get; set; }
        public OfferViewModel  Offer { get; set; }
        public int ReciverId { get; set; }
        public  UserViewModel Reciver { get; set; } // FK_Message_User1
      
        public int SenderId { get; set; }
        public UserViewModel Sender { get; set; } // FK_Message_User
        public MessageModelView lastmessage { get; set; }
        public int Count { get; set; }
        public string  Description { get; set; }
        public CustomerTypes UserType { get; set; }


    }
}
