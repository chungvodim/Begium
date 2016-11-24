using Begium.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Begium.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        public readonly IDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            if(filter != null)
            {
                return _dbSet.Where(filter);
            }
            else
            {
                return _dbSet;
            }
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual TEntity Find(params object[] keys)
        {
            return _dbSet.Find(keys);
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            try
            {
                _dbSet.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

            }
        }

        public virtual void Update(TEntity entity)
        {
            var entry = GetEntry(entity);
            entry.State = EntityState.Modified;
        }

        private DbEntityEntry<TEntity> GetEntry(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);
            if (entry == null)
            {
                _dbSet.Attach(entity);
                entry = _dbContext.Entry(entity);
            }

            return entry;
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Task.Run(() => Query(filter));
        }

        public virtual async Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return await Task.Run(() => Query(filter, orderBy));
        }

        public DbSqlQuery<TEntity> SqlQuery(string query)
        {
            return _dbSet.SqlQuery(query);
        }
    }
}