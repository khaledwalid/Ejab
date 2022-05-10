using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ejab.BAL.ModelViews.Statictics;
using Ejab.BAL.UnitOfWork;
using Ejab.BAL.Common;

namespace Ejab.BAL.Services.statistics
{
    public class statisticsService : IstatisticsService
    {
        IUnitOfWork _uow;
        ModelFactory factory;
        public statisticsService(IUnitOfWork uow)
        {
            this._uow = uow;
            factory = new ModelFactory();
        }
        public StaticticsViewModel AddStatistics(StaticticsViewModel model, int userId)
        {
            var statEntity = factory.Parse(model);
            if (model == null)
            {
                throw new Exception("005");
            }
            statEntity.FlgStatus = 1;
            statEntity.CreatedBy = userId;
            statEntity.CreatedOn = DateTime.Now.Date;
            _uow.Statistics.Add(statEntity);
            _uow.Commit();
            var interestModel = factory.Create(statEntity);
            return interestModel;
        }

        public IEnumerable<StaticticsViewModel> All()
        {
            var all = _uow.Statistics.GetAll(x => x.FlgStatus == 1, null, "").Select(s=> new StaticticsViewModel {Id=s.Id, AppDownloadsNo=s.AppDownloadsNo,CustomerNo=s.CustomerNo,OfferNo=s.OfferNo,TrucksOrdersNo=s.TrucksOrdersNo });
            if (all==null )
            {
                throw new Exception("No  Statistics Exists");
            }
            return all;
        }

        public StaticticsViewModel DeleteStatistics(int id, int userId)
        {
            var existed = _uow.Statistics.GetById(id);
          
            existed.FlgStatus = 0;
            existed.UpdatedBy = userId;
            existed.UpdatedOn = DateTime.Now;
            _uow.Statistics.Update(id, existed);
            _uow.Commit();
            var model = factory.Create(existed);
            return model;
        }

        public StaticticsViewModel EditStatistics(int id, StaticticsViewModel model, int userId)
        {
            var existed = _uow.Statistics.GetById(id);           
            existed.AppDownloadsNo = model.AppDownloadsNo;
            existed.CustomerNo = model.CustomerNo;
            existed.TrucksOrdersNo = model.TrucksOrdersNo;
            existed.FlgStatus = 1;
            existed.CreatedBy = userId;
            existed.CreatedOn = DateTime.Now;
            existed.OfferNo = model.OfferNo;
            _uow.Statistics.Update(id, existed);
            _uow.Commit();
            var statModel = factory.Create (existed);
            return statModel;
        }

        public StaticticsViewModel GetById(int id)
        {
            var existed = _uow.Statistics.GetById(id);
                
            var model = factory.Create(existed);
            return model;
        }

        public StaticticsViewModel top()
        {
            var all = _uow.Statistics.GetAll(x => x.FlgStatus == 1, null, "").Select(s => new StaticticsViewModel { AppDownloadsNo = s.AppDownloadsNo, CustomerNo = s.CustomerNo, OfferNo = s.OfferNo, TrucksOrdersNo = s.TrucksOrdersNo }).ToList().LastOrDefault();
            if (all == null)
            {
                throw new Exception("No  Statistics Exists");
            }
            return all;
        }
    }
}
