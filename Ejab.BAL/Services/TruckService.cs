using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;
using System.Drawing;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Configuration;
using Ejab.BAL.Helpers;
using System.Net.Http;
using System.Net;
using System.Configuration;

namespace Ejab.BAL.Services
{
    public class TruckService : ITruckService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        string  BaseURL = "";
        public TruckService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
            BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
          
        }

        public TrucksViewModel AddTruck(TrucksViewModel model, int UserId)
        {

            if (model == null)
            {

                throw new Exception("005");
            }
            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            {
                Directory.CreateDirectory(Phyiscalpath);
            }
            string converted = model.TruckImagePath.Substring(model.TruckImagePath.IndexOf(",") + 1);
            Image img = ImageHelper.Base64ToImage(converted);
            // string ext = Path.GetExtension(user.ProfileImgPath);
            string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + ".Jpeg";

            var path = Path.Combine(Phyiscalpath, filename);
            img.Save(path, ImageFormat.Jpeg);
            var entity = factory.Parse(model);
            entity.CreatedBy = UserId;
            entity.ParanetId = model.ParenetId;
            entity.TruckImagePath = filename;
            entity.FlgStatus = 1;
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.Truck.Add(entity);
            _uow.Commit();
            var type = _uow.TruckType.GetById(model.TruckTypeId);
            var truckModel = factory.Create(entity);
            var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            truckModel.TruckImagePath = ServerHelper.MapPathReverse(imagFullPath + entity.TruckImagePath).ToString();
            // truckModel.TruckImagePath =  entity.TruckImagePath;
            truckModel.TruckTypeName = type.Name;
            if (entity.ParanetId != null)
            {
                var parent = _uow.Truck.GetById(entity.ParanetId);
                truckModel.ParenetId = entity.ParanetId;
                truckModel.ParenetName = parent.Name;
            }
            return truckModel;
        }

        public IEnumerable<object> allParent()
        {
            var trucks = GetParentNodes().ToList();
            if (trucks == null)
            {
                throw new Exception("006");
            }
            return trucks;
        }

        public TrucksViewModel DeleteTruck(int id, int UserId)
        {
            var truck = _uow.Truck.GetById(id);
            if (truck == null)
            {

                throw new Exception("004");
            }
            if (truck.FlgStatus == 0)
            {
                throw new Exception("003");
            }
            truck.FlgStatus = 0;
            truck.UpdatedBy = UserId;
            truck.UpdatedOn = DateTime.Now.Date;
            _uow.Truck.Update(id, truck);
            _uow.Commit();
            var truckModel = factory.Create(truck);
            return truckModel;
        }

        public TrucksViewModel EditTruck(int id, TrucksViewModel model, int UserId)
        {
            var truck = _uow.Truck.GetById(id);
            var trucktypeid = truck.TruckType.Id;
            if (truck == null)
            {
                throw new Exception("004");
            }
            if (model == null)
            {
                throw new Exception("005");
            }
            if (model.TruckTypeId != trucktypeid)
            {
                truck.TypeId = model.TruckTypeId;
            }
            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            if (!Directory.Exists(Phyiscalpath))
            {
                Directory.CreateDirectory(Phyiscalpath);
            }
            string converted = model.TruckImagePath.Substring(model.TruckImagePath.IndexOf(",") + 1);
            Image img = ImageHelper.Base64ToImage(converted);
            // string ext = Path.GetExtension(user.ProfileImgPath);
            string filename = Guid.NewGuid().ToString() + DateTime.Today.ToString("ddMMyyyy") + model.Name + ".Jpeg";

            var path = Path.Combine(Phyiscalpath, filename);
            img.Save(path, ImageFormat.Jpeg);
            truck.Name = model.Name;
            truck.NameArb = model.NameArb;
            truck.ParanetId = model.ParenetId;
            truck.TruckImagePath = filename;
            truck.AvialableNo = model.AvialableNo;
            truck.Capacity = model.Capacity;
            truck.Description = model.Description;
            truck.Height = model.Height;
            truck.Weight = model.Weight;
            truck.Width = model.Width;
            truck.UpdatedBy = UserId;
            truck.UpdatedOn = DateTime.Now;
            _uow.Truck.Update(id, truck);
            _uow.Commit();
            var type = _uow.TruckType.GetById(model.TruckTypeId);
            var truckModel = factory.Create(truck);
            var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksImages/");
            truckModel.TruckImagePath = ServerHelper.MapPathReverse(imagFullPath + truck.TruckImagePath).ToString();
            truckModel.TruckTypeName = type.Name;
            truckModel.TruckTypeNameArb = type.NameArb;
            return truckModel;
        }

        public IEnumerable<TrucksViewModel> GetallTrucks()
        {
          //  var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksIcons/");
          
            var alltrucks = _uow.Truck.GetAll(x => x.FlgStatus == 1).ToList()
                .Select(T => new TrucksViewModel
                {
                    Id = T.Id,
                    truckType = factory.Create(T.TruckType),
                    TruckTypeNameArb = T.TruckType.NameArb,
                    TruckTypeName = T.TruckType.Name,
                    TruckTypeId =T.TruckType.Id,
                    Name = T.Name,
                    NameArb = T.NameArb,
                    ParenetId = T.ParanetId,
                    ParenetName = (T.Paranet != null) ? T.Paranet.Name : "",                   
                    TruckImagePath =  T.TruckImagePath !=null ?BaseURL+ T.TruckImagePath :null
                });

            return alltrucks;
        }

        public TrucksViewModel getById(int id)
        {
            var truck = _uow.Truck.GetById(id);
            if (truck == null)
            {

                throw new Exception("004");
            }
            var type = _uow.TruckType.GetById(truck.TypeId);
            var truckModel = factory.Create(truck);
            truckModel.TruckTypeName = type.Name;
            truckModel.TruckTypeNameArb = type.NameArb;
            return truckModel;
        }

        public IEnumerable<TrucksViewModel> TrucksByParent(int parentId)
        {

          //  var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksIcons/");

            List<TrucksViewModel> ChildTrucks = new List<TrucksViewModel>();
            var ChildNodes = _uow.Truck.GetAll(x => x.ParanetId == parentId && x.FlgStatus == 1).ToList();
            foreach (var item in ChildNodes)
            {
                TrucksViewModel truckObj = new TrucksViewModel();
                truckObj.Id = item.Id;
                truckObj.ParenetId  = item.ParanetId;
                truckObj.Name = item.Name;
                truckObj.NameArb = item.NameArb;
                truckObj.TruckTypeId = item.TypeId;
                truckObj.TruckTypeName = item.TruckType.Name;
                truckObj.TruckTypeNameArb = item.TruckType.NameArb;

                truckObj.TruckImagePath = item.TruckImagePath !=null ? item.TruckImagePath:null;
                if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                {
                    var childNode = GetChildsOfNode(item.Id);
                    truckObj.ChildModel = childNode;

                }
                ChildTrucks.Add(truckObj);
            }

            if (ChildTrucks == null)
            {
                throw new Exception("006");
            }

            List<TrucksViewModel> Trucks = new List<TrucksViewModel>();
            foreach (var item in ChildTrucks)
            {
                TrucksViewModel truckObj = new TrucksViewModel();
                truckObj.Id = item.Id;
                truckObj.Name = item.Name;//GetNetaqByMaslakh(item.Id);
                truckObj.TruckTypeId = item.TruckTypeId;
                truckObj.TruckTypeName = item.TruckTypeName;
                truckObj.TruckTypeNameArb = item.TruckTypeNameArb;
                truckObj.ParenetId = item.ParenetId;
                truckObj.ParenetName = item.ParenetName;
                truckObj.NameArb = item.NameArb;

                truckObj.TruckImagePath = item.TruckImagePath !=null ? BaseURL+ item.TruckImagePath  :null ;
                if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                {
                    truckObj.ChildModel = GetChildsOfNode(item.Id);
                }
                Trucks.Add(truckObj);
            }
            return Trucks;

        }

        public IEnumerable<TrucksViewModel> TrucksByType(int typeId)
        {
            var trucks = _uow.Truck.GetAll(x => x.TypeId == typeId && x.ParanetId  == null && x.FlgStatus == 1, null, "").ToList()
               .Select(t => new TrucksViewModel
               {
                   Id = t.Id,
                   Name = t.Name,
                   NameArb = t.NameArb,
                   ParenetId = t.ParanetId,
                   ParenetName = t.Paranet != null ? t.Paranet.Name : "",
                   ParenetNameArb = t.Paranet != null ? t.Paranet.NameArb : "",
                   TruckImagePath = t.TruckImagePath != null ? BaseURL + t.TruckImagePath : null
               ,
                   ChildModel = GetChildsOfNode(t.Id)
               });
            if (trucks == null)
            {
                throw new Exception("006");
            }
            return trucks;
            //var trucks = _uow.Truck.GetAll(x => x.TypeId == typeId&& x.Trucks.Any(y=>y.ParanetId !=null) &&x.FlgStatus == 1,null,"").ToList()
            //    .Select(t => new TrucksViewModel { Id = t.Id, Name = t.Name,NameArb=t.NameArb,
            //   ParenetId=t.ParanetId , ParenetName=t.Paranet !=null ?t.Paranet.Name :"",
            //    ParenetNameArb = t.Paranet != null ? t.Paranet.NameArb : "",
            //    TruckImagePath = t.TruckImagePath !=null ? BaseURL + t.TruckImagePath : null
            //    ,   ChildModel = GetChildsOfNode(t.Id)
            //    });
            //if (trucks == null)
            //{
            //    throw new Exception("006");
            //}
            //return trucks;
        }
        #region ParentChild
        public List<TrucksViewModel> GetChildsOfNode(int id)
        {          
            List<TrucksViewModel> Trucks = new List<TrucksViewModel>();
            var ChildNodes = _uow.Truck.GetAll(x => x.ParanetId == id && x.FlgStatus == 1).ToList();
            foreach (var item in ChildNodes)
            {
                TrucksViewModel truckObj = new TrucksViewModel();
                truckObj.Id = item.Id;
                truckObj.Name = item.Name;
                truckObj.NameArb = item.NameArb;
                truckObj.TruckTypeId = item.TypeId;
                truckObj.TruckTypeName = item.TruckType.Name;
                truckObj.TruckTypeNameArb = item.TruckType.NameArb;
                truckObj.ParenetId = item.ParanetId;
                truckObj.ParenetNameArb = item.Paranet != null ? item.Paranet.NameArb : "";
                truckObj.ParenetName = item.Paranet != null ? item.Paranet.Name: "";

                truckObj.TruckImagePath = item.TruckImagePath != null ? BaseURL + item.TruckImagePath: null;

                if (_uow.Truck.GetAll(x => x.ParanetId == item.Id && x.FlgStatus == 1).ToList().Count() > 0)
                {
                    truckObj.ChildModel = GetChildsOfNode(item.Id);
                }
                Trucks.Add(truckObj);
            }
            return Trucks;
        }
        public List<TrucksViewModel> GetParentNodes()
        {
           // var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksIcons/");
            List<TrucksViewModel> Trucks = new List<TrucksViewModel>();
            var truckslst = _uow.Truck.GetAll(x => x.ParanetId == null && x.FlgStatus == 1).ToList();
            foreach (var item in truckslst)
            {
                TrucksViewModel truckObj = new TrucksViewModel();
                truckObj.Id = item.Id;
                truckObj.Name = item.Name;
                truckObj.NameArb = item.NameArb;
                truckObj.TruckTypeId = item.TypeId;
                truckObj.TruckTypeName = item.TruckType.Name;
                truckObj.TruckTypeNameArb = item.TruckType.NameArb;
                truckObj.ParenetId = item.ParanetId;
                truckObj.ParenetNameArb = item.Paranet != null ? item.Paranet.NameArb : "";
                truckObj.ParenetName = item.Paranet != null ? item.Paranet.Name : "";


                truckObj.TruckImagePath = item.TruckImagePath != null ? BaseURL + item.TruckImagePath: null;
                Trucks.Add(truckObj);
            }
            return Trucks;
        }
        public object TruckDescription(int truckId)
        {
            var truckDescription = _uow.Truck.GetAll(x => x.FlgStatus == 1 && x.Id == truckId).Select(D => new { Weight = D.Weight, Width = D.Width, Capacity = D.Capacity, Height = D.Height, Description = D.Description });
            return truckDescription;
        }

        public IEnumerable<TrucksViewModel> GetallTrucks(string search, int? page)
        {
            if (search == null)
            {
                return _uow.Truck.GetAll(x => x.FlgStatus == 1).OrderByDescending(x => x.Name).Select(t => new TrucksViewModel { Id = t.Id, Name = t.Name, NameArb = t.NameArb, ParenetId = t.ParanetId, ParenetName = t.Paranet.Name, ParenetNameArb = t.Paranet.NameArb, Weight = t.Weight, Height = t.Height, Capacity = t.Capacity });
            }
            return _uow.Truck.GetAll(x => x.FlgStatus == 1).OrderByDescending(x => x.Name).Where(y => y.Name.ToLower().Contains(search)).Select(t => new TrucksViewModel { Id = t.Id, Name = t.Name, NameArb = t.NameArb, ParenetId = t.ParanetId, ParenetName = t.Paranet.Name, ParenetNameArb = t.Paranet.NameArb, Weight = t.Weight, Height = t.Height, Capacity = t.Capacity });
        }
        #endregion


        public TrucksViewModel AddTruckFromAdmin(int parentId, int typeId, TrucksViewModel model, int UserId, HttpPostedFileBase file)
        {
            var entity = factory.Parse(model);
            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/TrucksIcons/");
            {
                Directory.CreateDirectory(Phyiscalpath);
            }
            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                entity.TruckImagePath = ImageName;
            }

            entity.CreatedBy = UserId;
            if (parentId != 0)
                entity.ParanetId = parentId;
            else 
                entity.ParanetId = (Nullable<int>)null;
            
            entity.TypeId = typeId;
            entity.Name = model.Name;
            entity.NameArb = model.NameArb;
            entity.FlgStatus = 1;
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedBy = null;
            entity.UpdatedOn = null;
            _uow.Truck.Add(entity);
            _uow.Commit();
            var type = _uow.TruckType.GetById(model.TruckTypeId);
            var truckModel = factory.Create(entity);
            var imagFullPath = HttpContext.Current.Server.MapPath("~/TrucksIcons/");
            truckModel.TruckImagePath = ServerHelper.MapPathReverse(imagFullPath + entity.TruckImagePath);
        // truckModel.TruckImagePath = entity.TruckImagePath;
            // truckModel.TruckTypeName = type.Name;
            if (entity.ParanetId != null)
            {
                var parent = _uow.Truck.GetById(entity.ParanetId);
                truckModel.ParenetId = entity.ParanetId;
                truckModel.ParenetName = parent.Name;
                truckModel.ParenetNameArb = parent.NameArb;
            }
            return truckModel;
        }

        public TrucksViewModel EditTruckFromAdmin(int id, TrucksViewModel model, int UserId, HttpPostedFileBase file)
        {

            if (model == null)
            {
                throw new Exception("005");
            }
            var truck = _uow.Truck.GetById(id);
            if (truck == null)
            {
                throw new Exception("004");
            }
            var trucktypeid = truck.TruckType.Id;
            var entity = factory.Parse(model);
            var Phyiscalpath = HttpContext.Current.Server.MapPath("~/TrucksIcons/");
            {
                Directory.CreateDirectory(Phyiscalpath);
            }
            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                truck.TruckImagePath = ImageName;
            }

            if (model.TruckTypeId !=0)
            {
                truck.TypeId = model.TruckTypeId;
            }
            if (model.TruckTypeId == 0)
            {
                truck.TypeId = truck.TypeId;
            }
            truck.Name = model.Name;
            truck.NameArb = model.NameArb;
            //  truck.TruckImagePath  = model.TruckImagePath ;
            //truck.ParanetId = model.ParenetId;          
            //truck.AvialableNo = model.AvialableNo;
            //truck.Capacity = model.Capacity;
            //truck.Description = model.Description;
            //truck.Height = model.Height;
            //truck.Weight = model.Weight;
            //truck.Width = model.Width;
           
            truck.UpdatedBy = UserId;
            truck.UpdatedOn = DateTime.Now;
            _uow.Truck.Update(id, truck);
            _uow.Commit();
            //var type = _uow.TruckType.GetById(model.TruckTypeId);
            var truckModel = factory.Create(truck);
            //truckModel.TruckTypeName = type.Name;
            return truckModel;

        }

        public int TrucksCount()
        {
            return _uow.Truck.GetAll(x => x.FlgStatus == 1).ToList().Count();
        }

        public int trucks(int type, int month)
        {
            return _uow.Truck.GetAll(c => c.FlgStatus == 1).Where(y => y.CreatedOn.Month == month).ToList().Count();
        }

        public int getType(int truckId)
        {
            var truck = _uow.Truck.GetAll(x => x.FlgStatus == 1, null, "").Where(x => x.Id == truckId).SingleOrDefault();
           return  truck.TypeId;
        }

        public IEnumerable<object> allParentByType(int TypeId)
        {
            var trucks = GetParentNodes().Where(x=>x.TruckTypeId== TypeId).ToList();
            if (trucks == null)
            {
                throw new Exception("006");
            }
            return trucks;
        }
    }
}
