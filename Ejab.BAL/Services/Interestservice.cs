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
    public class Interestservice : IIntersetService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public Interestservice(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public InterestViewModel AddInterest(InterestViewModel model, int UserId)
        {
            if (model == null)
            {

                throw new Exception("005");
            }
            var interestEntity = factory.Parse(model);
            interestEntity.UserId = UserId;
            interestEntity.Date = DateTime.Now.Date;
            interestEntity.FlgStatus = 1;
            interestEntity.CreatedBy = UserId;
            interestEntity.CreatedOn = DateTime.Now.Date;
            var interestModel = factory.Create(interestEntity);
            var user = _uow.User.GetById(UserId);
            interestModel.User = factory.Create(user);
            if (model.TruckId.HasValue)
            {
                interestEntity.TruckId = model.TruckId.Value;
                if (checktruckExist(model.TruckId.Value, UserId))
                {
                    throw new Exception("105");
                }
                var truck = _uow.Truck.GetById(model.TruckId);
                interestModel.Truck = factory.Create(truck);
                interestModel.Count = userInterestsCount(UserId, "truck");
                _uow.Interest.Add(interestEntity);
                _uow.Commit();
            }
            else
            {
                interestEntity.TruckId = model.TruckId.HasValue ? model.TruckId : null;
            }
            if (model.RegionIds != null && model.RegionIds.Length > 0)
            {
                foreach (var item in model.RegionIds)
                {
                    if (!checkRegionExist(item.Value, UserId))
                    {
                        interestEntity.RegionId = item.Value;
                        _uow.Interest.Add(interestEntity);
                        _uow.Commit();
                    }
                    //else
                    //{
                    //    continue;
                    //    throw new Exception("106");
                       
                    //}
                }
                throw new Exception("108");
            }
            if (model.RegionIds != null)
            {
                foreach (var item in model.RegionIds)
                {
                    if (item.HasValue)
                    {
                        var region = _uow.Region.GetById(item.Value);
                        interestModel.Region = factory.Create(region);
                        interestModel.Count = userInterestsCount(UserId, "region");
                    }
                }

            }
            interestModel.InterestId = interestEntity.Id;
            return interestModel;

        }
        public int userInterestsCount(int UserId, string type)
        {
            int totalNumbers = 0;
            if (type == "truck")
            {
                totalNumbers = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.UserId == UserId).Where(y => y.Truck != null).Count();
            }
            if (type == "region")
            {
                totalNumbers = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.UserId == UserId).Where(y => y.Region != null).Count();
            }
            return totalNumbers;

        }

        public UserInterestsViewModel DeleteInterest(DeleteInterestViewModel ids, int UserId)
        {
            UserInterestsViewModel model = new UserInterestsViewModel();

            if (ids.TrucksIds != null && ids.TrucksIds.Count() > 0)
            {
                foreach (var id in ids.TrucksIds)
                {
                    var existedinteesrt = _uow.Interest.GetById(id);
                    if (existedinteesrt == null)
                    {
                        throw new Exception("004");
                    }
                    if (existedinteesrt.FlgStatus == 0)
                    {
                        throw new Exception("003");
                    }
                    //existedinteesrt.FlgStatus = 0;
                    //existedinteesrt.UpdatedBy = UserId;
                    //existedinteesrt.UpdatedOn = DateTime.Now.Date;
                    _uow.Interest.Delete(id, existedinteesrt);                   
                }

                _uow.Commit();

            }
            if (ids.RegionsIds != null && ids.RegionsIds.Count() > 0)
            {
                foreach (var id in ids.RegionsIds)
                {
                    var existedinteesrt = _uow.Interest.GetById(id);
                    if (existedinteesrt == null)
                    {
                        throw new Exception("004");
                    }
                    if (existedinteesrt.FlgStatus == 0)
                    {
                        throw new Exception("003");
                    }
                    //existedinteesrt.FlgStatus = 0;
                    //existedinteesrt.UpdatedBy = UserId;
                    //existedinteesrt.UpdatedOn = DateTime.Now.Date;
                    _uow.Interest.Delete(id, existedinteesrt);
                    // model.InterestId = existedinteesrt.Id;
                    //   model.Regions.Insert (0,factory.Create(existedinteesrt));
                    //  model.Regions.Add(factory.Create(existedinteesrt));
                }

                _uow.Commit();
            }


            return model;

        }

        public InterestViewModel EditInterest(int id, InterestViewModel model, int UserId)
        {

            var existedinteesrt = _uow.Interest.GetById(id);
            if (existedinteesrt == null)
            {
                throw new Exception("004");
            }
            if (model == null)
            {
                throw new Exception("005");
            }

            existedinteesrt.UserId = model.UserId;
            existedinteesrt.TruckId = model.TruckId.Value;
            foreach (var item in model.RegionIds)
            {
                if (item.HasValue)
                {
                    existedinteesrt.RegionId = item.Value;
                }

            }

            existedinteesrt.Date = model.Date;
            existedinteesrt.Notes = model.Notes;
            existedinteesrt.UpdatedBy = UserId;
            existedinteesrt.UpdatedOn = DateTime.Now.Date;
            _uow.Interest.Update(id, existedinteesrt);
            _uow.Commit();
            var interestModel = factory.Create(existedinteesrt);
            var truck = _uow.Truck.GetById(model.TruckId);
            interestModel.Truck = factory.Create(truck);
            foreach (var item in model.RegionIds)
            {
                if (item.HasValue)
                {
                    var region = _uow.Region.GetById(item.Value);
                    interestModel.Region = factory.Create(region);
                }

            }

            var user = _uow.User.GetById(model.UserId);
            interestModel.User = factory.Create(user);

            return interestModel;
        }

        public InterestViewModel Get(int id)
        {
            var inteesrt = _uow.Interest.GetById(id);
            if (inteesrt == null)
            {
                throw new Exception("004");
            }
            return factory.Create(inteesrt);
        }

        public UserInterestsViewModel GetAll(int userId, string type)
        {
            UserInterestsViewModel myModel = new UserInterestsViewModel();
            if (type.Contains("truck"))
            {
                //var trucks = _uow.Truck.GetAll(x => x.FlgStatus == 1, null, "Interests").Where(y=>y.Interests.Any(u=>u.UserId== userId)).Select(t => new UserInterestsViewModel { InterestId = t.Id, Trucks = t.Trucks.Select( c=> new TrucksViewModel { Id = c.Id, Name = c.Name }).AsQueryable()});
                //return trucks.ToList();
                var trucks = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.UserId == userId && x.RegionId == null, null, "").ToList();
                var itenested = trucks.Select(t =>
                    new InterestViewModel { InterestId = t.Id, Date = t.Date, TruckId = t.TruckId != null ? t.TruckId : 0, Truck = t.Truck != null ? factory.Create(t.Truck) : null }).ToList().Distinct().ToList();

                myModel.Trucks = itenested;
                return myModel;
            }
            if (type.Contains("region"))
            {
                var regions = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.UserId == userId && x.TruckId == null, null, "").ToList();
                //List<int?> Ids = new List<int?>();
                //foreach (var item in regions)
                //{
                //    if (item.RegionId.HasValue)
                //    {
                //        Ids.Add(item.RegionId.Value);
                //    }

                //}
                var itenested = regions.Select(t =>
                new InterestViewModel { InterestId = t.Id, Date = t.Date, Region = t.Region != null ? factory.Create(t.Region) : null }).ToList().Distinct().ToList();
                myModel.Regions = itenested;
                return myModel;
                //,RegionIds = t.RegionId.HasValue ? t.RegionId : 0
            }
            return null;

        }

        public IEnumerable<InterestViewModel> UserInterest(int userId)
        {
            var userinterest = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.UserId == userId, null, "").Select(i => new InterestViewModel { TruckId = i.TruckId, Truck = factory.Create(i.Truck), Region = factory.Create(i.Region), Date = i.Date, Notes = i.Notes });
            if (userinterest == null)
            {
                throw new Exception("004");
            }
            //RegionIds = i.RegionId.HasValue ? i.RegionId.Value : 0,
            return userinterest;
        }

        public IEnumerable<InterestViewModel> trucksInterest(int truckId)
        {
            var truckinterest = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.TruckId == truckId, null, "").Select(i => new InterestViewModel { UserId = i.UserId, User = factory.Create(i.User), TruckId = i.TruckId, Truck = factory.Create(i.Truck), Region = factory.Create(i.Region), Date = i.Date, Notes = i.Notes });
            if (truckinterest == null)
            {
                throw new Exception("004");
            }
            return truckinterest;
        }

        public IEnumerable<InterestViewModel> RegionInterest(int regionId)
        {
            var regionInterest = _uow.Interest.GetAll(x => x.FlgStatus == 1 && x.RegionId == regionId, null, "").Select(i => new InterestViewModel { UserId = i.UserId, User = factory.Create(i.User), TruckId = i.TruckId, Truck = factory.Create(i.Truck), Region = factory.Create(i.Region), Date = i.Date, Notes = i.Notes });
            if (regionInterest == null)
            {
                throw new Exception("004");
            }
            return regionInterest;
        }

        public bool checktruckExist(int truckId, int userId)
        {
            return _uow.Interest.GetAll(x => x.FlgStatus == 1, null, "").Any(x => x.TruckId == truckId && x.UserId == userId);

        }

        public bool checkRegionExist(int RegionId, int userId)
        {
            return _uow.Interest.GetAll(x => x.FlgStatus == 1, null, "").Any(x => x.RegionId == RegionId && x.UserId == userId);
        }
    }
}
