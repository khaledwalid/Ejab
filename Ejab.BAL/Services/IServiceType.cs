using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface IServiceType
    {
        IEnumerable<object> AllServiceType();
        ServiceTypeViewModel GetServiceTypebyId(int id);
        ServiceTypeViewModel AddServiceType(ServiceTypeViewModel model,int userId);
        ServiceTypeViewModel EditServiceTypes(int id,ServiceTypeViewModel model, int userId);
        ServiceTypeViewModel DeleteServiceTypes(int id,int userId);
    }
}
