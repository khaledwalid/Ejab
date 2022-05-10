using Ejab.BAL.CustomDTO;
using Ejab.BAL.Reository;
using Ejab.DAl;
using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
   public  interface IUserService 
    {
        User Verify(string username, string password);
        TokenDTO GenerateToken(User  user);
    }
}
