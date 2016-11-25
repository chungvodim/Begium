using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using Begium.Core;
using Begium.Data.Context;
using Begium.Core.Repositories;
using Begium.Core.UnitOfWork;
using Begium.Data.Entity;

namespace Begium.Dependencies
{
    public class EntityFrameworkFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(Component.For<IMainDbContext>()
                .ImplementedBy<MXEntities>()
                .LifestyleTransient());

            Kernel.Register(Component.For<ILogDbContext>()
                .ImplementedBy<MXLogsEntities>()
                .LifestyleTransient());

            Kernel.Register(Component.For(typeof(IUnitOfWorkAsync))
               .ImplementedBy(typeof(UnitOfWork))
               .LifestyleTransient());

            Kernel.Register(Component.For(typeof(IRepositoryAsync<>))
                    .ImplementedBy(typeof(BaseRepository<>))
                    .LifestyleTransient());
        }
    }
}