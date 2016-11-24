using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Begium.Data.Context;
using Begium.Data.Repositories;
using System.Data.Entity.Validation;
using Begium.Core;

namespace Begium.Data.UnitOfWork
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.
    /// </summary>
    public class UnitOfWork: BaseObject, IUnitOfWorkAsync
    {
        #region Private Fields

        /// <summary>
        /// A new instance of the <see cref="IDbContext"/> class
        /// </summary>
        private IDbContext _dataContext;
       
        /// <summary>
        /// ObjectContext is the top-level object that encapsulates a connection between the CLR and the database,
        /// serving as a gateway for Create, Read, Update, and Delete operations.
        /// </summary>
        private ObjectContext _objectContext;

        /// <summary>
        /// Represents a transaction to be performed at a data source, and is implemented by .NET Framework data providers that access relational databases.
        /// </summary>
        private DbTransaction _transaction;

        /// <summary>
        /// The flag will be used to identify the managed and unmanaged resources in during  dispose an UnitOfWork instance.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// A new instance of the <see cref="Hashtable"/> class that will be ma
        /// </summary>
        private Hashtable _repositories;

        #endregion Private Fields

        #region Constructor & Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">
        /// A new instance of the <see cref="IDbContext"/> class
        /// </param>
        public UnitOfWork(IDbContext context)
        {
            _dataContext = context;
            _disposed = false;
        }
        

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only

                try
                {
                    if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
                    {
                        _objectContext.Connection.Close();
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    log.Error(ex);
                    // do nothing, the objectContext has already been disposed
                }

                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }
            }

            // release any unmanaged objects
            // set the object references to null
         
            _disposed = true;
        }

        #endregion Constructor & Dispose

        /*
        Entity Framework by default wraps Insert, Update or Delete operation in a transaction, whenever you execute SaveChanges()
        EF starts a new transaction for each operation and completes the transaction when the operation finishes.When you execute another such operation, a new transaction is started.
        EF 6 has introduced database.BeginTransaction and Database.UseTransaction to provide more control over transactions.
        Refer: http://www.entityframeworktutorial.net/entityframework6/transaction-in-entity-framework.aspx
        */
        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        public void Save()
        {
            try
            {
                BeginTransaction();
                //saves all operations within one transaction
                _dataContext.SaveChanges();
                Commit();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        log.ErrorFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> number of objects written to the underlying database.
        /// </returns>
        public async Task SaveAsync()
        {
            try
            {
                BeginTransaction();
                //saves all operations within one transaction
                await _dataContext.SaveChangesAsync();
                Commit();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        log.ErrorFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                Rollback();
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Gets the repository that should be applied to the unit of work
        /// </summary>
        /// <typeparam name="TEntity">A generic type </typeparam>
        /// <returns>
        /// A new instance of <see cref="BaseRepository{TEntity}"/> class
        /// </returns>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return RepositoryAsync<TEntity>();
        }

        /// <summary>
        /// Gets the repository asynchronous that should be applied to the unit of work
        /// </summary>
        /// <typeparam name="TEntity">A generic type </typeparam>
        /// <returns>
        /// A new instance of <see cref="BaseRepository{TEntity}"/> class
        /// </returns>
        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {
            // Validate setting
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            // Gets the Entity's name
            var type = typeof(TEntity).Name;

            // check if exist state of the Entity in the repository by the Name.
            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)_repositories[type];
            }

            // Gets the type of repository.
            var repositoryType = typeof(BaseRepository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dataContext));

            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        #region Unit of Work Transactions

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            if (_objectContext.Connection.State == ConnectionState.Open) return;
            _objectContext.Connection.Open();
            _transaction = _objectContext.Connection.BeginTransaction();
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Commit()
        {
            _transaction.Commit();
            _transaction.Dispose();
            return true;
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }
        #endregion Unit of Work Transactions
    }
}
