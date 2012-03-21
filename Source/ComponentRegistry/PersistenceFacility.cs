using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using DDDIntro.Core;
using DDDIntro.Persistence.NHibernate;
using NHibernate;

namespace DDDIntro.ComponentRegistry
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(ctx => ctx.Resolve<NHibernate.Cfg.Configuration>().BuildSessionFactory()).LifestyleSingleton(),
                Component.For<ISession>().UsingFactoryMethod(ctx => ctx.Resolve<ISessionFactory>().OpenSession()).LifestyleScoped());

            Kernel.Register(
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(NHibernateRepository<>)).LifestyleScoped(),
                Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>().LifestyleSingleton(),
                Component.For<ISessionSharingUnitOfWorkFactory>().ImplementedBy<SessionSharingNHibernateUnitOfWorkFactory>().LifestyleScoped());
        }
    }
}