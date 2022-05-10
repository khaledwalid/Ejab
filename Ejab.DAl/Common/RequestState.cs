using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Common
{
    public enum RequestStates
    {
        Open = 1,      
        Accepted = 2,
        Rejected=5,
        Closed = 3,
        Cancelled = 4,
        Expired = 6
    }
}
