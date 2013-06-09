using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DDDIntro.ComponentRegistry;

namespace DDDIntro.WebAPI.TakeTwo.Infrastructure
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<SqlServerPersistenceFacility>();
            container.AddFacility<DomainServicesFacility>();
            container.AddFacility<ApplicationServicesFacility>();
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            // register all Fluent Validators
//            container.Register(
                //Component.For<IModelValidatorFactory>().ImplementedBy<WindsorFluentValidationValidatorFactory>().OverWrite(),
//                Component.For<IFluentAdapterFactory>().ImplementedBy<DefaultFluentAdapterFactory>(),
//                Classes.FromThisAssembly()
//                .BasedOn(typeof(IValidator<>))
//                .WithServiceBase()
//                .LifestyleTransient());
        }
    }
}