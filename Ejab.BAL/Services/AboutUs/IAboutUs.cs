using Ejab.BAL.ModelViews.AboutUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.AboutUs
{
  public   interface IAboutUs
    {

        AboutUsViewModel AddAboutUs(AboutUsViewModel model, int UserId);
        AboutUsViewModel EditAboutUs(int id, AboutUsViewModel model, int UserId);
        AboutUsViewModel DeleteAboutUs(int id, int UserId);
        AboutUsViewModel Get(int id);
        AboutUsViewModel GetAll();
      
    }
}
