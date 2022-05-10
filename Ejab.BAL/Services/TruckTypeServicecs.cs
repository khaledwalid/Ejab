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
    public class TruckTypeServicecs : ITruckTypeService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public TruckTypeServicecs(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public TruckTypeViewModel AddTruckType(TruckTypeViewModel model, int userId)
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
            _uow.TruckType.Add(entity);
            _uow.Commit();
            return factory.Create(entity);
        }

        public IEnumerable<TruckTypeViewModel> AllServiceType(string search,int? page)
        {
            if (search==null || string.IsNullOrEmpty(search))
            {
                return _uow.TruckType.GetAll(x => x.FlgStatus == 1, null, "Trucks").ToList().OrderByDescending(x => x.Name ).Select(x => new TruckTypeViewModel { TypeId  = x.Id, NameArb = x.NameArb,Name=x.Name , CreatedOn = x.CreatedOn, CreatedBy = x.CreatedBy, FlgStatus = x.FlgStatus, Trucks = x.Trucks.Select(t => new TrucksViewModel { Id = t.Id, Name = t.Name, ParenetName = (t.ParanetId.HasValue) ? t.Paranet.Name : "", ParenetId = t.ParanetId }) });
            }
            var types = _uow.TruckType.GetAll(x => x.FlgStatus == 1, null, "Trucks").ToList().OrderByDescending(x => x.NameArb ).Where(y=> y.NameArb.ToLower().Contains(search.ToLower())||  y.Name.ToLower().Contains(search.ToLower())).Select(x => new TruckTypeViewModel { TypeId =x.Id, NameArb = x.NameArb, Name=x.Name ,CreatedOn = x.CreatedOn, CreatedBy = x.CreatedBy, FlgStatus = x.FlgStatus, Trucks = x.Trucks.Select(t => new TrucksViewModel { Id=t.Id, Name = t.Name, ParenetName = (t.ParanetId.HasValue) ? t.Paranet .Name : "", ParenetId = t.ParanetId }) });
            return types;
        }

        public TruckTypeViewModel DeleteTruckTypes(int id, int userId)
        {
            var type = _uow.TruckType.GetById(id);
            if (type == null)
            {
                throw new Exception("004");
            }
            if (type.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            type.FlgStatus = 0;
            
            type.UpdatedBy = userId;
            type.UpdatedOn = DateTime.Now.Date;
            _uow.TruckType.Update(id, type);
            _uow.Commit();
            var typeModel = factory.Create(type);
            return typeModel;
        }

        public TruckTypeViewModel EditTruckTypes(int id, TruckTypeViewModel model, int userId)
        {
            if (model == null)
            {

                throw new Exception("005");
            }
            var type = _uow.TruckType.GetById(id);
            if (type == null)
            {
                throw new Exception("004");
            }
            type.NameArb = model.Name;
            type.Name = model.Name;
            type.UpdatedBy = userId;
            type.UpdatedOn = DateTime.Now;
            _uow.TruckType.Update(id, type);
            _uow.Commit();
            var typeModel = factory.Create(type);
            return typeModel;
        }

        public TruckTypeViewModel GetTruckTypebyId(int id)
        {
            var type = _uow.TruckType.GetById(id);
            if (type == null)
            {
                throw new Exception("004");
            }
            var model = factory.Create(type);
            return model;
        }

        public IEnumerable<TruckTypeViewModel> AllServiceType()
        {
            return _uow.TruckType.GetAll(x => x.FlgStatus == 1, null, "Trucks").ToList().Select(x => new TruckTypeViewModel { TypeId  = x.Id, NameArb = x.NameArb,Name=x.Name , CreatedOn = x.CreatedOn, CreatedBy = x.CreatedBy, FlgStatus = x.FlgStatus, Trucks = x.Trucks .Select(t => new TrucksViewModel { Id = t.Id, Name = t.Name, ParenetName = (t.ParanetId.HasValue) ? t.Paranet.Name : "", ParenetId = t.ParanetId }) });
        }
    }
}
