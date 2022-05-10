using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Reository
{
   public   interface IGenericRepository<T> where T :class
    {
        IQueryable<T> Table { get; }
        T Add(T entity);
        T Update(object id, T entity);
        void Delete(object id, T entity);
        IEnumerable <T> GetAll();
        T GetById(object id);
        T SearchByPredicate(Expression<Func<T, bool>> Predicate);
        IQueryable   <T> GetAllByPredicate(Expression<Func<T, bool>> filter = null,   string includeProperties = "");
        IQueryable <T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        int Count(T entity);
        void Deactivate(object id,bool Activation);
        void Remove(UserToken entity);
    }
}
