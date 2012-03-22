using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;
using DDDIntro.Persistence.NHibernate;
using DDDIntro.Web.Infrastructure.Persistence;
using FluentValidation;

namespace DDDIntro.Web.Infrastructure
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<PersistenceFacility>();
            container.AddFacility<DomainServicesFacility>();
            container.AddFacility<ApplicationServicesFacility>();

            // we're using nhibernate against SQL Server here
            container.Register(
                Component.For<INHibernateConfigurationProvider>()
                .ImplementedBy<SqlServerNHibernateConfigurationProvider>().LifestyleSingleton());

            // register all Fluent Validators
            container.Register(
                Classes.FromThisAssembly()
                .BasedOn(typeof(IValidator<>))
                .WithServiceBase()
                .LifestyleTransient());

            // register all controllers
            container.Register(Classes.FromThisAssembly()
                            .BasedOn<IController>()
                            .LifestyleTransient());
        }
    }
}