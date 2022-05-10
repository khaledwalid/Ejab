using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ejab.DAl.Models;
using Ejab.BAL.Reository;
using Ejab.DAl.Common;
using Ejab.BAL.ModelViews;

namespace Ejab.BAL.Services
{
    public class SysLogBAL : ISysLog
    {
        IGenericRepository<SysLog> _repo;
        public SysLogBAL()
        {
            //_repo = Repo;
        }
        public IQueryable<SysLog> Table
        {
            get
            {
                return _repo.Table.Where(b => b.FlgStatus == 1);
            }
        }

        public SysLog Add(SysLog entity)
        {
            return _repo.Add(entity);
        }

        public void AddNewLog(ActionData actiontype, string desc, int createdby)
        {
            SysLog slog = new SysLog();
            slog.ActionId = (int)actiontype;
            slog.Description = desc;
            slog.CreatedBy = createdby;
            slog.CreatedOn = DateTime.Now;
            slog.FlgStatus = 1;
            _repo.Add (slog);
        }

        public void attach(SysLog entity)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchText = null)
        {
            throw new NotImplementedException();
        }

        public int Count(SysLog entity)
        {
            return _repo.Count(entity);
        }

        public int countbyCondition(SysLog entity, Expression<Func<SysLog, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Deactivate(object id, bool Activation)
        {
            throw new NotImplementedException();
        }

        //public int Count(string searchText = null)
        //{
        //    UnitOfWork .UnitOfWork  UOW = new UnitOfWork.UnitOfWork();
        //    var users = UOW.us.GetAll();
        //    var logs = UOW.SysLog.GetAll();

        //    var qLogObjs = (from a in logs
        //                    join u in users on a.CreatedBy.Value equals u.Id
        //                    select new LogDTO()
        //                    {
        //                        Id = a.Id,
        //                        ActionId = a.ActionId,
        //                        ActionName = a.ActionId == 2 ? "اضافة" : a.ActionId == 3 ? "تعديل" : a.ActionId == 4 ? "حذف" : "",
        //                        Description = a.Description,
        //                        CreatedBy = a.CreatedBy,
        //                         CreatedOn = a.CreatedOn,
        //                        FlgStatus = a.FlgStatus
        //                    }
        //        );

        //    if (!string.IsNullOrEmpty(searchText))
        //        qLogObjs = qLogObjs.Where(a => a.ActionName.Contains(searchText) || a.Description.Contains(searchText) || a.CreatedOn.ToString() == searchText);

        //    var resC = qLogObjs.Count();

        //    return resC;
        //}

        public void Delete(object id, SysLog entity)
        {
            _repo.Delete(id, entity);
        }

        public IQueryable<SysLog> GetAll()
        {
             return null ;
        }

        public IQueryable<SysLog> GetAll(Expression<Func<SysLog, bool>> filter = null, Func<IQueryable<SysLog>, IOrderedQueryable<SysLog>> orderBy = null, string includeProperties = "")
        {
            return null;
        }

        public IQueryable<SysLog> GetAllByPredicate(Expression<Func<SysLog, bool>> filter = null, string includeProperties = "")
        {
            return _repo.GetAllByPredicate();
        }

        public SysLog GetById(object id)
        {
            return _repo.GetById(id);
        }

        public List<LogDTO> GetLogData(int pageIndex, int pageSize, string searchText = null)
        {
            throw new NotImplementedException();
        }

        //public List<LogDTO> GetLogData(int pageIndex, int pageSize, string searchText = null)
        //{
        //    try
        //    {
        //      UnitOfWork UOW = new UnitOfWork();
        //        var users = UOW.User.GetAll();
        //        var logs = UOW.SysLog.GetAll();

        //        List<LogDTO> LogObjs = null;

        //        var qLogObjs = (from a in logs
        //                        join u in users on a.CreatedBy.Value equals u.Id
        //                        select new LogDTO()
        //                        {
        //                            Id = a.Id,
        //                            ActionId = a.ActionId,
        //                            ActionName = a.ActionId == 2 ? "اضافة" : a.ActionId == 3 ? "تعديل" : a.ActionId == 4 ? "حذف" : "",
        //                            Description = a.Description,
        //                            CreatedBy = a.CreatedBy,
        //                           CreatedOn = a.CreatedOn,
        //                            FlgStatus = a.FlgStatus
        //                        }
        //                    ).OrderByDescending(a => a.CreatedOn).AsEnumerable();

        //        if (!string.IsNullOrEmpty(searchText))
        //            qLogObjs = qLogObjs.Where(a => a.ActionName.Contains(searchText) || a.Description.Contains(searchText) ||
        //                 a.CreatedByName.Contains(searchText) || a.CreatedOn.ToString() == searchText);

        //        LogObjs = qLogObjs.Skip(pageIndex * pageSize).Take(pageSize).ToList();

        //        return LogObjs;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public SysLog SearchByPredicate(Expression<Func<SysLog, bool>> Predicate)
        {
            return _repo.SearchByPredicate(Predicate);
        }

        public SysLog Update(object id, SysLog entity)
        {
            return _repo.Update(id, entity);
        }
    }
}
