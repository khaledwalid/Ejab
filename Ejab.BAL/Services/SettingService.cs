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
    public class SettingService : Isettingcs
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        string BaseURL = "";
        public SettingService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public SettingViewModel add(SettingViewModel setting, int AdminId)
        {
            if (setting == null)
            {

                throw new Exception("005");
            }
            var entity = factory.Parse(setting);
            entity.AdminPeriod = DateTime.Now.Date;
            entity.CreatedBy = AdminId;           
            entity.FlgStatus = 1;
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.Setting .Add(entity);
            _uow.Commit();         
            var truckModel = factory.Create(entity);
          
            return truckModel; ;
        }

        public SettingViewModel Delete(int Id,  int AdminId)
        {
            var Extsetting = _uow.Setting .GetById(Id);
            if (Extsetting == null)
            {
                throw new Exception("004");
            }
            if (Extsetting.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            Extsetting.FlgStatus = 0;

            Extsetting.UpdatedBy = AdminId;
            Extsetting.UpdatedOn = DateTime.Now.Date;
            _uow.Setting .Update(Id, Extsetting);
            _uow.Commit();
            var typeModel = factory.Create(Extsetting);
            return typeModel;
        }

        public SettingViewModel Edit(int Id, SettingViewModel setting, int AdminId)
        {
            if (setting == null)
            {

                throw new Exception("005");
            }
            var ExtSetting = _uow.Setting .GetById(Id);
            if (ExtSetting == null)
            {
                throw new Exception("004");
            }
            ExtSetting.AdminPeriod = DateTime.Now.Date;
            ExtSetting.AdminPeriod = setting.AdminPeriod;
            ExtSetting.ExpirDayies  = setting.ExpirDayies;
            ExtSetting.MaxExpirDayies  = setting.MaxExpirDayies;
            ExtSetting.MaxAcceptNo = setting.MaxAcceptNo;
            ExtSetting.UpdatedBy = AdminId;
            ExtSetting.UpdatedOn = DateTime.Now;
            _uow.Setting .Update(Id, ExtSetting);
            _uow.Commit();
            var typeModel = factory.Create(ExtSetting);
            return typeModel;
        }

        public IEnumerable<SettingViewModel> GetAll()
        {
            return _uow.Setting .GetAll(x => x.FlgStatus == 1, null, "").ToList().Select(x => new SettingViewModel  { Id  = x.Id, AdminPeriod  = x.AdminPeriod , MaxAcceptNo  = x.MaxAcceptNo , ExpirDayies  = x.ExpirDayies , MaxExpirDayies  = x.MaxExpirDayies});
        }

        public SettingViewModel GetById(int Id)
        {
            var type = _uow.Setting .GetById(Id);
            if (type == null)
            {
                throw new Exception("004");
            }
            var model = factory.Create(type);
            return model;
        }
    }
}
