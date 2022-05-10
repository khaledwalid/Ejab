using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface IDevicesInterface
    {
        int GetDeviceByUserId(int UserId);
        int GetDeviceData(int deviceId);
    }
}
