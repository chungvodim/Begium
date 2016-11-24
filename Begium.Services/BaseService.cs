using System.Collections.Generic;
using System.Linq;
using Begium.Data.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Begium.Data.Repositories;
using Begium.Core;

namespace Begium.Services
{
    /// <summary>
    /// Base service with common logic
    /// </summary>
    public abstract class BaseService<TEntity> : BaseObject, IBaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Unit of work, delivery class must be implement to use it
        /// </summary>
        protected IUnitOfWorkAsync unitOfWork;
        private IRepository<TEntity> _dataRepository;
        private IRepositoryAsync<TEntity> _dataRepositoryAsync;

        public BaseService(IUnitOfWorkAsync unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _dataRepository = unitOfWork.Repository<TEntity>();
            _dataRepositoryAsync = unitOfWork.RepositoryAsync<TEntity>();
        }

        public virtual void Dispose()
        {
            if (null != unitOfWork)
            {
                unitOfWork.Dispose(true);
            }
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dataRepository.Find(keyValues);
        }

        public virtual void Insert(TEntity entity)
        {
            _dataRepository.Insert(entity);
            unitOfWork.Save();
        }

        public virtual bool Save()
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.Save();
                unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                unitOfWork.Rollback();
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync()
        {
            try
            {
                unitOfWork.BeginTransaction();
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                unitOfWork.Rollback();
                return false;
            }
        }

        public virtual void Update(TEntity entity)
        {
            _dataRepository.Update(entity);
            unitOfWork.Save();
        }

        public virtual TEntity First(Expression<Func<TEntity, bool>> filter)
        {
            return _dataRepository.Query(filter).First();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return _dataRepository.Query(filter).FirstOrDefault();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? _dataRepository.Count(x => true) : _dataRepository.Count(filter);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, bool ascending = true, int page = 1, int count = int.MaxValue)
        {
            if (filter == null)
                filter = (x => true);

            var resultSet = _dataRepository.Query(filter);

            if (orderBy != null)
                //order
                resultSet = ascending ? resultSet.OrderBy(orderBy) : resultSet.OrderByDescending(orderBy);

            //pagination
            resultSet = resultSet.Skip((page - 1) * count).Take(count);
            return resultSet;

        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dataRepositoryAsync.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Task.Run(() => FirstOrDefault(filter));
        }

        public virtual async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, bool @ascending = true, int page = 1, int count = Int32.MaxValue)
        {
            return await Task.Run(() => Get(filter, orderBy, ascending, page, count));
        }

    }
}