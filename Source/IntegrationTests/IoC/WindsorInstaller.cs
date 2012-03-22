using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;
using DDDIntro.Persistence.NHibernate;
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

            // using an in-memory sqllite database for speed and to encourage
            // database engine portability
            container.Register(
                Component.For<INHibernateConfigurationProvider>()
                .ImplementedBy<InMemoryNHibernateConfigurationProvider>()
                .LifestyleSingleton());

            // we're using nhibernate against an in-memory database that gets lost each session
            // export the schema on the session's connection when a session is created
            // as sqllite will not have a database for each session otherwise!
            container.Kernel.ComponentCreated += (model, instance) =>
            {
                if ((instance is ISession))
                {
                    var session = instance as ISession;
                    InMemoryNHibernateConfigurationProvider.InitialiseDatabase(
                        container.Resolve<NHibernate.Cfg.Configuration>(),
                        session);
                }
            };
        }
    }
}