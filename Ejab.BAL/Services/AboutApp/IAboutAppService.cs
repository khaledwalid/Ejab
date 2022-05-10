using Ejab.BAL.ModelViews.AboutApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.AboutApp
{
  public   interface IAboutAppService
    {
        AboutAppViewModel AddAboutApp(AboutAppViewModel model, int UserId);
        AboutAppViewModel EditAboutApp(int id, AboutAppViewModel model, int UserId);
        AboutAppViewModel DeleteAboutApp(int id, int UserId);
        AboutAppViewModel Get(int id);
        AboutAppViewModel GetAll();
    }
}
