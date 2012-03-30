using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Nancy;
using Nancy.Validation.FluentValidation;

namespace DDDIntro.WebAPI.Infrastructure
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<SqlServerPersistenceFacility>();
            container.AddFacility<DomainServicesFacility>();
            container.AddFacility<ApplicationServicesFacility>();

            // register all Fluent Validators
            container.Register(
                Component.For<IFluentAdapterFactory>().ImplementedBy<DefaultFluentAdapterFactory>(),
                Classes.FromThisAssembly()
                .BasedOn(typeof(IValidator<>))
                .WithServiceBase()
                .LifestyleTransient());


        }
    }
}