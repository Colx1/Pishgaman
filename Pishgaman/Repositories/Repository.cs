using Microsoft.EntityFrameworkCore;
using Pishgaman.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pishgaman.Repositories
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageCountRec = 0, int pageNum = 0);

        TEntity GetByID(int id);

        int Count();

        void Insert(TEntity model);
        void Update(TEntity model);
        void Update(List<TEntity> model);

        void Delete(int id);
        void Delete(TEntity model);

        bool Save();
    }

    public class DBRepository<TDbContext, TEntity, TKey> :
        IRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
    {
        TDbContext db;
        public DBRepository(TDbContext _db) => db = _db;
        public List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageCountRec = 0, int pageNum = 0)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            foreach (var incProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperties);

            if (orderBy != null)
                query = orderBy(query);

            if (pageCountRec > 0 && pageNum > 0)
                query = query.Skip((pageNum - 1) * pageCountRec).Take(pageCountRec);

            return query.ToList();
        }

        public TEntity GetByID(int id) => db.Set<TEntity>().Find(id);

        public int Count() => db.Set<TEntity>().Count();

        public void Insert(TEntity model) => db.Add(model);

        public void Update(TEntity model) => db.Update(model);

        public void Update(List<TEntity> model) => db.UpdateRange(model);

        public void Delete(int id)
        {
            TEntity tmodel = db.Set<TEntity>().Find(id);
            Delete(tmodel);
        }

        public void Delete(TEntity model)
        {
            if (db.Entry(model).State == EntityState.Detached)
                db.Set<TEntity>().Attach(model);

            db.Set<TEntity>().Remove(model);
        }

        public bool Save()
        {
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    db.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}
