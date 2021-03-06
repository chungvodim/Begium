﻿using System.Threading;
using System.Threading.Tasks;
using Begium.Core.Repositories;

namespace Begium.Core.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> number of objects written to the underlying database.
        /// </returns>
        Task SaveAsync();

        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class;
    }
}
