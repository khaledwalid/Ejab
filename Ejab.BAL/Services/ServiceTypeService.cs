using Ejab.BAL.Common;
using Ejab.BAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews;

namespace Ejab.BAL.Services
{
   public  class ServiceTypeService: IServiceType
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public ServiceTypeService(IUnitOfWork uow, IUserService UserService)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }

        public IEnumerable<object> AllServiceType()
        {
           var types = _uow.ServiceType.GetAll(x => x.FlgStatus == 1).ToList().Select(x => new { Id=x.Id, Name = x.Name, CreatedOn = x.CreatedOn, createdBy = x.CreatedBy, Status = x.FlgStatus });
            if (types==null)
            {
                throw new Exception("006");
            }
            return types;
        }

        public ServiceTypeViewModel GetServiceTypebyId(int id)
        {
            var type = _uow.ServiceType.GetById(id);
            if (type == null)
            {

                throw new Exception("006");
            }
            var model = factory.Create(type);
            return model;
        }

        public ServiceTypeViewModel AddServiceType(ServiceTypeViewModel model, int userId)
        {
           
            if (model == null)
            {
                throw new Exception("005");
            }
            var entity = factory.Parse(model);
            entity.CreatedBy = userId;
            entity.CreatedOn = DateTime.Now;
            entity.FlgStatus = 1;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.ServiceType.Add(entity);
            _uow.Commit();
          return  factory.Create(entity);
        }

        public ServiceTypeViewModel EditServiceTypes(int id, ServiceTypeViewModel model, int userId)
        {
            if (model == null)
            {
                throw new Exception("005");
            }
            var type = _uow.ServiceType.GetById(id);
            if (type == null)
            {
                throw new Exception("004");
            }
            type.Name = model.Name;
            type.UpdatedBy = userId;
            type.UpdatedOn = DateTime.Now;
            _uow.ServiceType.Update(id, type);
            _uow.Commit();
            var typeModel = factory.Create(type);
            return typeModel;
        }

        public ServiceTypeViewModel DeleteServiceTypes(int id, int userId)
        {
            var type = _uow.ServiceType.GetById(id);
            if (type == null)
            {
               throw new Exception ("004");
            }
            if (type.FlgStatus == 0)
            {
               throw new Exception ("003");
            }
            type.FlgStatus = 0;
            type.UpdatedBy = userId;
            type.UpdatedOn = DateTime.Now.Date;
            _uow.ServiceType.Update(id, type);
            _uow.Commit();
            var typeModel = factory.Create(type);
            return typeModel;
        }
    }
}
