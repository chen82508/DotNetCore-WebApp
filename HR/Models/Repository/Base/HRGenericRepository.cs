using HR.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace HR.Models.Repository.Base
{
    public class HRGenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _DbContext;
        public HRGenericRepository(DbContext db) { _DbContext = db; }

        public IQueryable<TEntity> GetAllData()
        {
            return _DbContext.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetDataByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _DbContext.Set<TEntity>().Where(expression);
        }

        public void Insert(TEntity entity)
        {
            _DbContext.Set<TEntity>().Add(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            _DbContext.Set<TEntity>().AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            _DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _DbContext.Set<TEntity>().UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _DbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public void SaveChange()
        {
            try
            {
                _DbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                //導入錯誤日誌
                //throw;
                foreach (EntityEntry entity in _DbContext.ChangeTracker.Entries())
                {
                    entity.Reload();
                }
                _DbContext.SaveChanges();
            }

            foreach (EntityEntry entity in _DbContext.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}
