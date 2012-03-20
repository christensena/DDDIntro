using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using DDDIntro.Core;
using DDDIntro.Persistence.NHibernate;
using NHibernate;

namespace ComponentRegistry
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(ctx => ctx.Resolve<NHibernate.Cfg.Configuration>().BuildSessionFactory()).LifestyleSingleton(),
                Component.For<ISession>().UsingFactoryMethod(ctx => ctx.Resolve<ISessionFactory>().OpenSession()).LifestylePerWebRequest());

            Kernel.Register(
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(NHibernateRepository<>)).LifestylePerWebRequest(),
                Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>().LifestyleSingleton(),
                Component.For<ISessionSharingUnitOfWorkFactory>().ImplementedBy<SessionSharingNHibernateUnitOfWorkFactory>().LifestylePerWebRequest());
        }
    }
}