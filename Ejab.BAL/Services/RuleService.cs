using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;

namespace Ejab.BAL.Services
{
    public class RuleService : IRuleService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public RuleService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public RuleViewModel AddRule(RuleViewModel rule, int userId)
        {
            if (rule == null)
            {
                throw new Exception("005");
            }
            var entity = factory.Parse( rule);
            entity.FlgStatus = 1;
            entity.CreatedBy = userId;
            entity.CreatedOn = DateTime.Now.Date;
            _uow.Rule.Add(entity);
            _uow.Commit();
            var model = factory.Create(entity);
            return model;
        }

        public IEnumerable<RuleViewModel> AllRules(string search=null)
        {
            return _uow.Rule.GetAll(x => x.FlgStatus == 1).Where(x=> search==null ||  x.Name.ToLower().Contains(search)).Select(r=> new RuleViewModel {Id=r.Id,Name=r.Name,Description=r.Description ,DescriptionEng=r.DescriptionEng });
        }

    

        public RuleViewModel DeleteRule(int id, int userId)
        {
            var rule = _uow.Rule.GetById(id);
            rule.FlgStatus = 0;
            rule.UpdatedBy = userId;
            rule.UpdatedOn = DateTime.Now.Date;
            _uow.Rule.Update( id,rule);
            _uow.Commit();
            var model = factory.Create( rule);
            return model;
        }

        public RuleViewModel EditRule(int id, RuleViewModel rule, int userId)
        {
            if (rule == null)
            {
                throw new Exception("005");
            }
            var existedentity = _uow.Rule .GetById(id);
           
            existedentity.Name  = rule.Name ;
            existedentity.Description  = rule.Description ;
            existedentity.DescriptionEng = rule.DescriptionEng;
            existedentity.FlgStatus = 1;
            existedentity.UpdatedBy = userId;
            existedentity.UpdatedOn = DateTime.Now.Date;
            _uow.Rule .Update(id, existedentity);
            _uow.Commit();
            var ruleModel = factory.Create(existedentity);
            return ruleModel;
        }

        public IEnumerable<UserViewModel> UsersInRule(int ruleId)
        {
            return
                _uow.User.GetAll(x => x.FlgStatus == 1&& x.IsAdmin==true  && x.UserRules != null && x.UserRules.Any(y => y.Id == ruleId))
                    .Select(u=> new UserViewModel
            {
                        Id=u.Id,FirstName=u.FirstName,LastName=u.LastName,Email = u.Email,Mobile=u.Mobile 
            }
        );
        }

        public RuleViewModel GetRule(int ruleId)
        {
            var rule = _uow.Rule.GetById(ruleId);
            var model = factory.Create(rule);
            return model;
        }

        public IEnumerable<RuleViewModel> ExistedRules(int UserId)
        {
            var rules = _uow.Rule.GetAll(x => x.FlgStatus == 1).Where(y =>  y.Users.Any(u => u.Id == UserId)).Select(r=> new RuleViewModel { Id=r.Id,Name=r.Name,Description=r.Description ,DescriptionEng=r.DescriptionEng }).ToList();
            return rules;
        }
    }
}
