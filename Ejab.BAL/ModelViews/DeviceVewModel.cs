using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class DeviceVewModel
    {
        public int  Id { get; set; }
        public string SerialNumber { get; set; } // SerialNumber (length: 200)
        public string DeviceToken { get; set; } // DeviceToken (length: 500)
        public string DeviceType { get; set; } // DeviceType (length: 100)
        public int? UserDeviceId { get; set; } // UserDevice_Id
    }
}
