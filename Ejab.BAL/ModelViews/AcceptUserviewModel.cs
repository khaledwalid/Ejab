using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public class AcceptUserviewModel
    {
        public int UserId { get; set; }
        public string  UserName { get; set; }
        public string  UserPhone { get; set; }
        public string UserImage{ get; set; }
        public string  acceptedDate { get; set; }
    }
}
