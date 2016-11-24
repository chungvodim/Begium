using System.Collections.Generic;
using System.Linq;
using Begium.Data.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Begium.Services
{
    /// <summary>
    /// Base service with common logic
    /// </summary>
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Unit of work, delivery class must be implement to use it
        /// </summary>
        protected IUnitOfWorkAsync UnitOfWork;

        public virtual void Dispose()
        {
            if (null != UnitOfWork)
            {
                UnitOfWork.Dispose(true);
            }
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await UnitOfWork.RepositoryAsync<TEntity>().FindAsync(keyValues);
        }

        public TEntity Find(params object[] keyValues)
        {
            return UnitOfWork.Repository<TEntity>().Find(keyValues);
        }
        
        public virtual void Insert(TEntity entity)
        {
            UnitOfWork.Repository<TEntity>().Insert(entity);
            UnitOfWork.Save();
        }

        public virtual bool Save()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Save();
                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                await UnitOfWork.SaveAsync();
                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                return false;
            }
        }

        public virtual void Update(TEntity entity)
        {
            UnitOfWork.Repository<TEntity>().Update(entity);
            UnitOfWork.Save();
        }
    }
}