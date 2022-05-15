using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.CustomDTO
{
   public  class TokenDTO
    {
        public int user_id;
        public string token;
        public string type;
        public string full_name;
        public string email;
        public string issued_on;
        public string expires_on;
        public string[] roles;
    }
}
