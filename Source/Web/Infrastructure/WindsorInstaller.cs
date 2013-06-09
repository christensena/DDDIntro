using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;
using DDDIntro.Web.Infrastructure.Bootstrapping;
using FluentValidation;

namespace DDDIntro.Web.Infrastructure
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<SqlServerPersistenceFacility>();
            container.AddFacility<DomainServicesFacility>();
            container.AddFacility<ApplicationServicesFacility>();

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn(typeof (IBootstrapper))
                    .WithServiceBase()
                    .LifestyleSingleton());

            // for bootstrappers
            container.Register(
                Component.For<WindsorControllerFactory>().ImplementedBy<WindsorControllerFactory>().LifestyleSingleton(),
                Component.For<WindsorValidatorFactory>().ImplementedBy<WindsorValidatorFactory>().LifestyleSingleton());

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