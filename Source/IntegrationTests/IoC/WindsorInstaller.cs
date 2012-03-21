using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;

namespace DDDIntro.IntegrationTests.IoC
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<PersistenceFacility>();
            container.AddFacility<DomainServicesFacility>();
            container.AddFacility<ApplicationServicesFacility>();

            // we're using nhibernate against a temporary sqllite db here
            container.Register(
                Component.For<NHibernate.Cfg.Configuration>().UsingFactoryMethod(x => TempDatabaseNHibernateConfigurationProvider.GetTempDatabaseConfiguration()).LifestyleSingleton());
        }
    }
}