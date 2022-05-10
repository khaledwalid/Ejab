using Ejab.DAl;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Linq;
using Ejab.DAl.Models;

namespace Ejab.BAL.Reository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private  EjabContext context;
        private DbSet<T> table;
        public GenericRepository(EjabContext _ctx)
        {
            context = _ctx;
            table = context.Set<T>();

        }
        //public GenericRepository()
        //{

        //}
        public T Add(T entity)
        {
            return table.Add(entity);
        }
        public void Delete(object id, T entity)
        {
            var existing = table.Find(id);
            if (existing != null)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
        }
        public IEnumerable<T> GetAll()
        {
                return table;
        }
        public IQueryable<T> GetAllByPredicate(Expression<Func<T, bool>> Predicate)
        {
            return table.Where(Predicate);
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public T SearchByPredicate(Expression<Func<T, bool>> Predicate)
        {
            return table.Where(Predicate).FirstOrDefault();
        }
        public T Update(object id, T entity)
        {
            //if (id == null)
            //    return null;

            //var existing = table.Find(id);
            //if (existing != null)
            //{
                table.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
               
            //}
            return entity;

        }
        public void Dispose()
        {
            if (context !=null)
            {
                context = null;
                context.Dispose();
                GC.SuppressFinalize(this);

            }
        }
        private bool disposed = false;

        public IQueryable<T> Table
        {
            get
            {
                return table;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public IQueryable<T> GetAllByPredicate(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = table ;
            if (filter != null)
            {
                query = table .Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;

        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = table ;
            if (filter != null)
            {
                query = table.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
                return query;

            }
            return query;
        }

        public int Count(T entity)
        {
            return table.Count();
        }

        public void Deactivate(object id, bool Activation)
        {
            var existing = table.Find(id);
            if (existing != null)
            {

                context.Entry(existing).State = EntityState.Modified ;
            }
        }

        public void Remove(UserToken entity)
        {
            context.UserTokens.Remove(entity);
        }
    }
}
