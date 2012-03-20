using System;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.Application.Services;
using DDDIntro.Core;
using DDDIntro.Domain.Services;
using DDDIntro.Domain.Services.CommandHandlers;
using DDDIntro.Domain.Services.Factories;
using DDDIntro.Domain.Services.QueryHandlers;
using DDDIntro.Persistence.NHibernate;
using FluentValidation;
using NHibernate;

namespace DDDIntro.Web.Infrastructure
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            RegisterPersistenceComponents(container);
            RegisterDomainServices(container);

            container.Register(
                Classes.FromThisAssembly()
                .BasedOn(typeof(IValidator<>))
                .WithServiceBase()
                .LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                            .BasedOn<IController>()
                            .LifestyleTransient());

        }

        private static void RegisterPersistenceComponents(IWindsorContainer container)
        {
            container.Register(
                Component.For<NHibernate.Cfg.Configuration>().UsingFactoryMethod(x => NHibernateConfigurationProvider.GetDatabaseConfiguration()).LifestyleSingleton(),
                Component.For<ISessionFactory>().UsingFactoryMethod(ctx => ctx.Resolve<NHibernate.Cfg.Configuration>().BuildSessionFactory()).LifestyleSingleton(),
                Component.For<ISession>().UsingFactoryMethod(ctx => ctx.Resolve<ISessionFactory>().OpenSession()).LifestylePerWebRequest());

            container.Register(
                Component.For(typeof (IRepository<>)).ImplementedBy(typeof (NHibernateRepository<>)).LifestylePerWebRequest(),
                Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>().LifestyleSingleton(),
                Component.For<ISessionSharingUnitOfWorkFactory>().ImplementedBy<SessionSharingNHibernateUnitOfWorkFactory>().LifestylePerWebRequest());
        }

        private static void RegisterDomainServices(IWindsorContainer container)
        {
            container.Register(
                Classes.FromAssemblyContaining<RecordDeliveryCommandHandler>()
                    .BasedOn(typeof (ICommandHandler<>)).WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<PlayersForCountryQueryHandler>()
                    .BasedOn(typeof (IQueryHandler<,>)).WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<MatchesForPlayerQueryHandler>()
                    .BasedOn(typeof(IQueryHandler<,>)).WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<CountryFactory>()
                    .BasedOn(typeof(IEntityFactory)).WithServiceSelf()
                    .LifestyleTransient());

            container.Register(
                Classes.FromAssemblyContaining<TestDataGenerator>()
                    .Pick() // filter by namespace or something later
                    .LifestyleTransient());
        }
    }
}