using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Begium.Core;
using Begium.Dependencies;

namespace Begium.Installers
{
    public class RepositoriesIntaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        { 
            container.AddFacility<EntityFrameworkFacility>();
        }
    }
}