using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Helpers
{
  public class SmsMessaging
    {
        public SmsMessaging()
        {

        }
        public void Send()
        {
        }
        public int   CreateRandom()
        {
            Random r = new Random();
           return   r.Next(5);
        }
    }
}
