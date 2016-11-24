using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Begium.Data.Context
{
    public interface IDbContext : IDisposable
    {
        //
        // Summary:
        //     Gets a System.Data.Entity.Infrastructure.DbEntityEntry object for the given
        //     entity providing access to information about the entity and the ability to
        //     perform actions on the entity.
        //
        // Parameters:
        //   entity:
        //     The entity.
        //
        // Returns:
        //     An entry for the entity.
        DbEntityEntry Entry(object entity);
        //
        // Summary:
        //     Gets a System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> object for
        //     the given entity providing access to information about the entity and the
        //     ability to perform actions on the entity.
        //
        // Parameters:
        //   entity:
        //     The entity.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     An entry for the entity.
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        //
        // Summary:
        //     Saves all changes made in this context to the underlying database.
        //
        // Returns:
        //     The number of state entries written to the underlying database. This can
        //     include state entries for entities and/or relationships. Relationship state
        //     entries are created for many-to-many relationships and relationships where
        //     there is no foreign key property included in the entity class (often referred
        //     to as independent associations).
        //
        // Exceptions:
        //   System.Data.Entity.Infrastructure.DbUpdateException:
        //     An error occurred sending updates to the database.
        //
        //   System.Data.Entity.Infrastructure.DbUpdateConcurrencyException:
        //     A database command did not affect the expected number of rows. This usually
        //     indicates an optimistic concurrency violation; that is, a row has been changed
        //     in the database since it was queried.
        //
        //   System.Data.Entity.Validation.DbEntityValidationException:
        //     The save was aborted because validation of entity property values failed.
        //
        //   System.NotSupportedException:
        //     An attempt was made to use unsupported behavior such as executing multiple
        //     asynchronous commands concurrently on the same context instance.
        //
        //   System.ObjectDisposedException:
        //     The context or connection have been disposed.
        //
        //   System.InvalidOperationException:
        //     Some error occurred attempting to process entities in the context either
        //     before or after sending commands to the database.
        int SaveChanges();
        //
        // Summary:
        //     Asynchronously saves all changes made in this context to the underlying database.
        //
        // Returns:
        //     A task that represents the asynchronous save operation.  The task result
        //     contains the number of state entries written to the underlying database.
        //     This can include state entries for entities and/or relationships. Relationship
        //     state entries are created for many-to-many relationships and relationships
        //     where there is no foreign key property included in the entity class (often
        //     referred to as independent associations).
        //
        // Exceptions:
        //   System.Data.Entity.Infrastructure.DbUpdateException:
        //     An error occurred sending updates to the database.
        //
        //   System.Data.Entity.Infrastructure.DbUpdateConcurrencyException:
        //     A database command did not affect the expected number of rows. This usually
        //     indicates an optimistic concurrency violation; that is, a row has been changed
        //     in the database since it was queried.
        //
        //   System.Data.Entity.Validation.DbEntityValidationException:
        //     The save was aborted because validation of entity property values failed.
        //
        //   System.NotSupportedException:
        //     An attempt was made to use unsupported behavior such as executing multiple
        //     asynchronous commands concurrently on the same context instance.
        //
        //   System.ObjectDisposedException:
        //     The context or connection have been disposed.
        //
        //   System.InvalidOperationException:
        //     Some error occurred attempting to process entities in the context either
        //     before or after sending commands to the database.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported.
        //     Use 'await' to ensure that any asynchronous operations have completed before
        //     calling another method on this context.
        Task<int> SaveChangesAsync();
        //
        // Summary:
        //     Asynchronously saves all changes made in this context to the underlying database.
        //
        // Parameters:
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task
        //     to complete.
        //
        // Returns:
        //     A task that represents the asynchronous save operation.  The task result
        //     contains the number of state entries written to the underlying database.
        //     This can include state entries for entities and/or relationships. Relationship
        //     state entries are created for many-to-many relationships and relationships
        //     where there is no foreign key property included in the entity class (often
        //     referred to as independent associations).
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     Thrown if the context has been disposed.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported.
        //     Use 'await' to ensure that any asynchronous operations have completed before
        //     calling another method on this context.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        //
        // Summary:
        //     Returns a System.Data.Entity.DbSet<TEntity> instance for access to entities
        //     of the given type in the context and the underlying store.
        //
        // Type parameters:
        //   TEntity:
        //     The type entity for which a set should be returned.
        //
        // Returns:
        //     A set for the given entity type.
        //
        // Remarks:
        //     Note that Entity Framework requires that this method return the same instance
        //     each time that it is called for a given context instance and entity type.
        //     Also, the non-generic System.Data.Entity.DbSet returned by the System.Data.Entity.DbContext.Set(System.Type)
        //     method must wrap the same underlying query and set of entities. These invariants
        //     must be maintained if this method is overridden for anything other than creating
        //     test doubles for unit testing.  See the System.Data.Entity.DbSet<TEntity>
        //     class for more details.
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set")]
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        //
        // Summary:
        //     Returns a non-generic System.Data.Entity.DbSet instance for access to entities
        //     of the given type in the context and the underlying store.
        //
        // Parameters:
        //   entityType:
        //     The type of entity for which a set should be returned.
        //
        // Returns:
        //     A set for the given entity type.
        //
        // Remarks:
        //     Note that Entity Framework requires that this method return the same instance
        //     each time that it is called for a given context instance and entity type.
        //     Also, the generic System.Data.Entity.DbSet<TEntity> returned by the System.Data.Entity.DbContext.Set(System.Type)
        //     method must wrap the same underlying query and set of entities. These invariants
        //     must be maintained if this method is overridden for anything other than creating
        //     test doubles for unit testing.  See the System.Data.Entity.DbSet class for
        //     more details.
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set")]
        DbSet Set(Type entityType);
    }
}
