using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.Rest.Models.Security
{
  public   class LoginVM
    {

        public string username;
        public string Email;
        public string password;
        public string grant_type;
    }
    public class TokenVM
    {
        public int user_id { get; set; }
        public string username { get; set; }
       public string Email { get; set; }
        //public int region_id { get; set; }
        //public int survey_id { get; set; }
        public long issued_on { get; set; }
        public long expires_on { get; set; }
       public string[] roles { get; set; }
    }
}
