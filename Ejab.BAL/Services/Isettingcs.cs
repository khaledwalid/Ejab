using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface Isettingcs
    {
        SettingViewModel add(SettingViewModel setting, int AdminId);
        SettingViewModel Edit(int Id,SettingViewModel setting, int AdminId);
        SettingViewModel Delete(int Id, int AdminId);
        IEnumerable<SettingViewModel> GetAll();
        SettingViewModel GetById(int Id);
    }
}
