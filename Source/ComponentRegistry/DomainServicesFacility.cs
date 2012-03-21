using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using DDDIntro.Domain.Services;
using DDDIntro.Domain.Services.CommandHandlers;
using DDDIntro.Domain.Services.Factories;
using DDDIntro.Domain.Services.QueryHandlers;
using DDDIntro.Persistence.NHibernate.QueryHandlers;

namespace DDDIntro.ComponentRegistry
{
    public class DomainServicesFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Classes.FromAssemblyContaining<RecordDeliveryCommandHandler>()
                    .BasedOn(typeof(ICommandHandler<>)).WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<PlayersForCountryQueryHandler>()
                    .BasedOn(typeof(IQueryHandler<,>)).WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<MatchesForPlayerQueryHandler>()
                    .BasedOn(typeof(IQueryHandler<,>)).WithServiceBase()
                    .LifestyleTransient(),
                Classes.FromAssemblyContaining<CountryFactory>()
                    .BasedOn(typeof(IEntityFactory)).WithServiceSelf()
                    .LifestyleTransient());
        }
    }
}