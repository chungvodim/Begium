using Begium.Data.Repositories;

namespace Begium.Data.UnitOfWork
{
    using System;

    /// <summary>
    /// The UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        void Dispose(bool disposing);

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        void Save();

        /// <summary>
        /// Returns a non-generic IRepository instance for access to entities of the given type in the context,
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity for which a get should be returned.
        /// </typeparam>
        /// <returns>
        /// A new instance of the <see>
        ///         <cref>IRepository</cref>
        ///     </see>
        ///     class
        /// </returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class ;

        void BeginTransaction();

        bool Commit();

        void Rollback();
    }

}
