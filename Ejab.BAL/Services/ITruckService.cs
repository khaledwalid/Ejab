using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ejab.BAL.Services
{
  public   interface ITruckService
    {
       IEnumerable <TrucksViewModel> GetallTrucks();
        TrucksViewModel getById(int id);
        TrucksViewModel AddTruck(TrucksViewModel model ,int UserId);
        TrucksViewModel EditTruck(int id,TrucksViewModel model, int UserId);
        TrucksViewModel DeleteTruck(int id,int UserId);
        IEnumerable<TrucksViewModel> TrucksByType(int typeId);
        IEnumerable<object> allParent();
        IEnumerable<TrucksViewModel> TrucksByParent(int parentId);
        object TruckDescription(int truckId);
        IEnumerable<TrucksViewModel> GetallTrucks(string search,int? page);
        TrucksViewModel AddTruckFromAdmin(int parentId, int typeId, TrucksViewModel model, int UserId, HttpPostedFileBase file);
        TrucksViewModel EditTruckFromAdmin(int id,TrucksViewModel model, int UserId, HttpPostedFileBase file);
        int TrucksCount();
        int trucks(int type,int  month);
        int getType(int truckId);
        IEnumerable<object> allParentByType(int TypeId);



    }
}
