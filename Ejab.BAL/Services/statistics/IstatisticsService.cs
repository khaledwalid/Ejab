using Ejab.BAL.ModelViews.Statictics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.statistics
{
  public   interface IstatisticsService
    {
        StaticticsViewModel AddStatistics(StaticticsViewModel model,int userId);
        StaticticsViewModel EditStatistics(int id,StaticticsViewModel model, int userId);
        StaticticsViewModel DeleteStatistics(int id, int userId);
        IEnumerable<StaticticsViewModel> All();
       StaticticsViewModel top();
        StaticticsViewModel GetById(int id);
    }
}
