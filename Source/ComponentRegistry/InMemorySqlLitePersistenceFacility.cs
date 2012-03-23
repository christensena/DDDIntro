using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using DDDIntro.Persistence.NHibernate;
using DDDIntro.Persistence.NHibernate.NHibernateConfigurationProviders;

namespace DDDIntro.ComponentRegistry
{
    public class InMemorySqlLitePersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.AddFacility<PersistenceFacility>();

            Kernel.Register(
                Component.For<INHibernateConfigurationProvider>()
                    .ImplementedBy<InMemoryNHibernateConfigurationProvider>()
                    .LifestyleSingleton());
        }
    }
}