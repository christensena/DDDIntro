using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using DDDIntro.Application.Services;

namespace ComponentRegistry
{
    public class ApplicationServicesFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(
                Classes.FromAssemblyContaining<TestDataGenerator>()
                    .Pick() // filter by namespace or something later
                    .LifestyleTransient());
        }
    }
}