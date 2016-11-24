using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Begium.Data.Repositories
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Asynchronously finds an entity with the given primary key values.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found. </param>
        /// <returns> A <see cref="Task"/> that represents the asynchronous find operation,
        /// and <see cref="Task"/> result contains the entity found, or null.</returns>
        Task<TEntity> FindAsync(params object[] keyValues);

        /// <summary>
        /// Asynchronously finds an entity with the given primary key values.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <param name="keyValues">The values of the primary key for the entity to be found. </param>
        /// <returns> A <see cref="Task"/> that represents the asynchronous find operation,
        /// and <see cref="Task"/> result contains the entity found, or null.</returns>
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);

        /// <summary>
        /// Asynchronously return a list of given entity
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
    }
}
