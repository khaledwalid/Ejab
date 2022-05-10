using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface IRegionService
    {
        RegionModelView AddRegion(RegionModelView model, int UserId);
        RegionModelView EditRegion(int id, RegionModelView model, int UserId);
        RegionModelView DeleteRegion(int id, int UserId);
        RegionModelView getById(int id);
        IEnumerable<RegionModelView> allRegions(string search);
        IEnumerable<RegionModelView> regionByParent(int parentid);
        IEnumerable<RegionModelView> allRegionswithSearch(int id);
        IEnumerable<RegionModelView> Regions(string search);
       bool   CheckRegionExist(string name);
        int  GetRegionIdByName(string name);
    }

}
