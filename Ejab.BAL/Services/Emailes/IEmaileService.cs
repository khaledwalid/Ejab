using Ejab.BAL.ModelViews.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services.Emailes
{
  public interface IEmaileService
    {
        IQueryable< EmailSubscriptionViewModel> AllEmailes();
       
        EmailSubscriptionViewModel GetEmailById(int id);
        EmailSubscriptionViewModel AddEmaile(EmailSubscriptionViewModel model, int userId);
        EmailSubscriptionViewModel EditEmaile(int id, EmailSubscriptionViewModel model, int userId);
        EmailSubscriptionViewModel DeleteEmaile(int id, int userId);
    }
}
