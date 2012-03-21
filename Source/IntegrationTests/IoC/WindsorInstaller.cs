using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;
using NHibernate;

namespace DDDIntro.IntegrationTests.IoC
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<PersistenceFacility>();
            container.AddFacility<DomainServicesFacility>();
            container.AddFacility<ApplicationServicesFacility>();

            container.Register(
                Component.For<NHibernate.Cfg.Configuration>().UsingFactoryMethod(x => TempDatabaseNHibernateConfigurationProvider.GetTempDatabaseConfiguration()).LifestyleSingleton());

            // we're using nhibernate against an in-memory database that gets lost each session
            // export the schema on the session's connection when a session is created
            // as sqllite will not have a database for each session otherwise!
            container.Kernel.ComponentCreated += (model, instance) =>
            {
                if (!(instance is ISession)) return;
                var session = instance as ISession;
                TempDatabaseNHibernateConfigurationProvider.InitialiseDatabase(
                    container.Resolve<NHibernate.Cfg.Configuration>(),
                    session);
            };
        }
    }
}