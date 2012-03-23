using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using DDDIntro.Persistence;
using DDDIntro.Persistence.NHibernate;
using NHibernate;

namespace DDDIntro.ComponentRegistry
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Component.For<NHibernate.Cfg.Configuration>().UsingFactoryMethod(kernel => kernel.Resolve<INHibernateConfigurationProvider>().GetDatabaseConfiguration()).LifestyleSingleton(),
                Component.For<ISessionFactory>().UsingFactoryMethod(ctx => ctx.Resolve<NHibernate.Cfg.Configuration>().BuildSessionFactory()).LifestyleSingleton(),
                Component.For<ISession>().UsingFactoryMethod(ctx => ctx.Resolve<ISessionFactory>().OpenSession()).LifestyleScoped());

            Kernel.Register(
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(NHibernateRepository<>)).LifestyleScoped(),
                Component.For<IIsolatedUnitOfWorkFactory>().ImplementedBy<NHibernateIsolatedUnitOfWorkFactory>().LifestyleSingleton(),
                Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>().LifestyleScoped());
        }
    }
}