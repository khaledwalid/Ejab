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
    public class RegionService : IRegionService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public RegionService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public bool CheckRegionExist(string name)
        {

          return   _uow.Region.GetAll(x => x.FlgStatus == 1,null,"").Any(y => y.Name.ToLower().Equals(name.ToLower()));
          
        }
        public RegionModelView AddRegion(RegionModelView model, int UserId)
        {
            if (CheckRegionExist(model.Name) )
            {
                throw new Exception("81");

            }
            if (model.Name == null || model.Name == "")
            {
                throw new Exception("86");
            }
            var entity = factory.Parse(model);
            entity.FlgStatus = 1;
            entity.parantId = model.ParanetId;
            entity.CreatedOn = DateTime.Now.Date;
            entity.CreatedBy = UserId;
            _uow.Region.Add(entity);
            _uow.Commit();
            var regionmodel = factory.Create(entity);
            if (model.ParanetId != null)
            {
                var parent = _uow.Region.GetById(model.ParanetId);
                model.ParaneName = parent.Name;
            }
            return regionmodel;
        }

        public RegionModelView DeleteRegion(int id, int UserId)
        {
            var region = _uow.Region.GetById(id);
            if (region == null)
            {
                throw new Exception("004");
            }
            if (region.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            region.FlgStatus = 0;
            region.UpdatedBy = UserId;
            region.UpdatedOn = DateTime.Now.Date;
            _uow.Region.Update(id, region);
            _uow.Commit();
            var regionModel = factory.Create(region);
            return regionModel;
        }

        public RegionModelView EditRegion(int id, RegionModelView model, int UserId)
        {
            var region = _uow.Region.GetById(id);
            if (region == null)
            {
                throw new Exception("004");
            }
            if (model == null)
            {
                throw new Exception("005");
            }
            region.Name = model.Name;
            region.parantId = model.ParanetId;
            region.UpdatedBy = UserId;
            region.UpdatedOn = DateTime.Now;
            _uow.Region.Update(id, region);
            _uow.Commit();
            var regionModel = factory.Create(region);
            if (regionModel.ParanetId != null)
            {
                var parent = _uow.Region.GetById(regionModel.ParanetId);
                regionModel.ParaneName = parent.Name;
            }
            return regionModel;
        }

        public RegionModelView getById(int id)
        {
            var region = _uow.Region.GetAll(x=>x.FlgStatus==1 && x.Id==id).FirstOrDefault();
            if (region == null)
            {

                throw new Exception("004");
            }
            var regionModel = factory.Create(region);
            if (regionModel.ParanetId != null)
            {
                var parent = _uow.Region.GetById(regionModel.ParanetId);
                regionModel.ParaneName = parent.Name;

            }
            return regionModel;
        }

        public IEnumerable<RegionModelView> regionByParent(int parentid)
        {
            var cities = _uow.Region.GetAll(x => x.FlgStatus == 1 && x.parantId == parentid).Select(c => new RegionModelView { Id = c.Id, Name = c.Name });
            return cities;
        }

        public IEnumerable<RegionModelView> allRegions(string search)
        {
            var regions = GetParentNodes();

            if (search !=null )
            {
                return regions.Where(x => x.Name.ToLower().Contains(search));
            }
            return regions;
           
        }

        public IEnumerable<RegionModelView> allRegionswithSearch(int id)
        {
          return   GetChildsOfNode(id);
           
        }
        public List<RegionModelView> GetChildsOfNode(int id)
        {
            List<RegionModelView> cities = new List<RegionModelView>();
            var ChildNodes = _uow.Region.GetAll(x => x.parantId == id && x.FlgStatus == 1).ToList();
            foreach (var item in ChildNodes)
            {
                RegionModelView cityObj = new RegionModelView();
                cityObj.Id = item.Id;
                cityObj.Name = item.Name;
                if (_uow.Region.GetAll(x => x.parantId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                {
                    cityObj.ChildModel = GetChildsOfNode(item.Id);
                }
                cities.Add(cityObj);
            }
            return cities;
        }
        public List<RegionModelView> GetParentNodes()
        {
            List<RegionModelView> Regions = new List<RegionModelView>();
            var regionslst = _uow.Region.GetAll(x => x.parantId == null && x.FlgStatus == 1).ToList();
            foreach (var item in regionslst)
            {
                RegionModelView cityObj = new RegionModelView();
                cityObj.Id = item.Id;
                cityObj.Name = item.Name;
                Regions.Add(cityObj);
            }
            return Regions;
        }

        public IEnumerable<RegionModelView> Regions(string search)
        {
            int regionid = 0;
            int.TryParse(search, out regionid);
            if (search==null )
            {
                return _uow.Region.GetAll(x => x.FlgStatus == 1&& x.Name !=null, null, "").ToList().Select(r => new RegionModelView { Id = r.Id, Name = r.Name ,NameArb=r.NameArb}).Distinct().ToList();
            }
            var regions = _uow.Region.GetAll(x => x.FlgStatus == 1, null, "").Where(x=>x.Name.Contains(search) || x.Name.StartsWith(search) || x.Id.Equals(regionid));
            var regionModel = regions.ToList().Select(r=> new RegionModelView {Id=r.Id,Name=r.Name, NameArb = r.NameArb });
            return regionModel;
        }

        public int GetRegionIdByName(string name)
        {
            return _uow.Region.GetAll(x => x.FlgStatus == 1, null, "").Where(y => y.Name.ToLower() .Equals(name.ToLower())).FirstOrDefault().Id;
        }
    }
}
