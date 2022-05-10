using Ejab.BAL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
  public   interface IRuleService
    {
        RuleViewModel AddRule(RuleViewModel rule,int userId);
        RuleViewModel EditRule(int id,RuleViewModel rule, int userId);
        RuleViewModel DeleteRule(int id,int userId);
     
      IEnumerable<RuleViewModel> AllRules( string search);
      IEnumerable<UserViewModel> UsersInRule(int ruleId);
        RuleViewModel GetRule(int ruled);
      IEnumerable< RuleViewModel> ExistedRules(int UserId);
    }
}
