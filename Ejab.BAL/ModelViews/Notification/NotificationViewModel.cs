using Ejab.BAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Notification
{
  public   class NotificationViewModel
    {
       
       
        public int NotyId { get; set; }
        public int SenderId { get; set; }       
        public UserViewModel SenderUser { get; set; }
        [Required]
        public int ReceiverId { get; set; }      
        public UserViewModel ReciverUser { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DeviceType DeviceType { get; set; }
        [Required]
        private string _deviceToken;
      

        [Required]
        public string Title { get; set; }
        [Required]
        public string  Body { get; set; }
        public string Description { get; set; }
        public string BodyArb { get; set; }
        public string BodyEng { get; set; }
        public string TitleArb { get; set; }
        public string TitleEng { get; set; }
        public bool Seen { get; set; }
        public List<string > registration_ids { get; set; }
        public NotificationType Type { get; set; }
        public List< int> Recivers { get; set; }

        public string DeviceToken
        {
            set
            {
                registration_ids[0] = _deviceToken;
            }
        }
    }
}
