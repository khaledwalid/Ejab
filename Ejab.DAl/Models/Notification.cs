
using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ejab.DAl.Models
{
   public  class Notification:BaseModel 
    {
        [Required]
        public int SenderId { get; set; }
      
        public User  SenderUser { get; set; }
        [Required]
        public int ReceiverId { get; set; }
       
        public User  ReciverUser { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DeviceType DeviceType { get; set; }       
        [Required]
        public string  Title { get; set; }
        [Required]
        public string  Body { get; set; }
        public bool  Seen { get; set; }
    }
}
