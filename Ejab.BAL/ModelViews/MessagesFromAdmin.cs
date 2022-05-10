using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public  class MessagesFromAdmin
    {
        public int MessageId { get; set; }
       
        public System.DateTime Date { get; set; } 
        public string SendingTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "MessageTitleIsRequired")]
        public string  MessageTitle { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "MessageDescriptionIsRequired")]
        public string Description { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Global),
        //   ErrorMessageResourceName = "MessageSenderIsRequired")]
        public int SenderId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "MessageReciverIsRequired")]
        public int ReciverId { get; set; }
        public MessageType MessageType { get; set; }
        public bool? Status { get; set; } // Status
       
    }
}
