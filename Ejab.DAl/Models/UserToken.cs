using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
   public  class UserToken:BaseModel
    {
        public int UserId { get; set; }
        public string Fcmtoken { get; set; }
        public virtual  ICollection<User> Users { get; set; }
    }
}
