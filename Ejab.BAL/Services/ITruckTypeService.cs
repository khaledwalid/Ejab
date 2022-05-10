using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface ITruckTypeService
    {
        IEnumerable<TruckTypeViewModel> AllServiceType();
        IEnumerable<TruckTypeViewModel> AllServiceType(string search,int? page);
        TruckTypeViewModel GetTruckTypebyId(int id);
        TruckTypeViewModel AddTruckType(TruckTypeViewModel model, int userId);
        TruckTypeViewModel EditTruckTypes(int id, TruckTypeViewModel model, int userId);
        TruckTypeViewModel DeleteTruckTypes(int id, int userId);
    }
}
