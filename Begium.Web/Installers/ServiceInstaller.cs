using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Begium.Core;
using Begium.Core.UnitOfWork;

namespace Begium.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyNamed("Begium.Services")
                .Where(type => type.Name.EndsWith("Service"))
                .WithService
                .DefaultInterfaces()
                .LifestylePerWebRequest());
        }
    }
}