using Begium.Core;
using Begium.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Begium.Data.Repositories
{
    public class BaseRepository<TEntity> : BaseObject, IRepositoryAsync<TEntity> where TEntity : class
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

            _dbSet.Add(entity);
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
        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.Count(filter);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual void DeleteByID(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            _dbSet.Remove(entityToDelete);
        }

        public virtual void DeleteByID(long id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            _dbSet.Remove(entityToDelete);
        }

        #region advance

        /// <summary>
        /// update modified fields
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="newEntity"></param>
        public virtual void UpdateModifiedFields(int entityID, TEntity newEntity)
        {
            TEntity entity = Find(entityID);
            var entry = _dbContext.Entry(entity);
            var properties = entity.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    var value = property.GetValue(entity);
                    var newValue = property.GetValue(newEntity);
                    if (value != newValue)
                    {
                        entry.Property(property.Name).CurrentValue = newValue;
                    }
                }
            }
        }

        /// <summary>
        /// Update records that match condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="act"></param>
        public virtual void UpdateEach(Expression<Func<TEntity, Boolean>> predicate, Action<TEntity> act)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (TEntity local in Queryable.Where<TEntity>(query, predicate))
            {
                act(local);
            }
        }
        /// <summary>
        /// Set new value for selected properties of attached Entity in current context
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="attachedEntity">Entity that already attached in current db context</param>
        /// <param name="newEntity">Entity that will provide new data</param>
        /// <param name="propertiesToUpdate">Properties need to be updated of attached entity</param>
        public void UpdateSelectFields(TEntity attachedEntity, TEntity newEntity, params Expression<Func<TEntity, Object>>[] propertiesToUpdate)
        {

            if (propertiesToUpdate == null)
            {
                throw new ArgumentException("propertiesToUpdate is null");
            }

            if (attachedEntity == null)
            {
                throw new ArgumentException("attachedEntity is null");
            }

            var entry = _dbContext.Entry(attachedEntity);
            if (entry == null)
            {
                throw new ArgumentException(attachedEntity.ToString() + " does not exist in current context");
            }

            Type newType = newEntity.GetType();

            foreach (var item in propertiesToUpdate)
            {
                MemberExpression memExp = item.Body as MemberExpression;
                if (memExp == null)
                {
                    UnaryExpression unExp = item.Body as UnaryExpression;
                    if (unExp == null)
                    {
                        throw new InvalidOperationException("Selected property " + item.Name + " is invalid");
                    }
                    memExp = (MemberExpression)unExp.Operand;
                }

                string propertyName = memExp.Member.Name;
                Object newValue = newType.GetProperties().Single(x => x.Name == propertyName).GetValue(newEntity, null);
                entry.Property(propertyName).CurrentValue = newValue;
            }

        }

        protected virtual List<DTO> Search<DTO>(IEnumerable<DTO> list, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                string[] oKeySearch = keyword.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string key in oKeySearch)
                {
                    list = list.Where(x => GetSearchedField(x).Contains(key.ToLower()));
                }
            }
            return list.ToList();
        }

        private string GetSearchedField<DTO>(DTO model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var properties = model.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(model);
                    if (value != null)
                    {
                        sb.Append(value.ToString().ToLower() + " ");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Unable to get searched field: {0}", ex);
                return "";
            }

        }
        #endregion

    }
}