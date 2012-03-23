using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DDDIntro.Web.Infrastructure.Bootstrapping;

[assembly: WebActivator.PostApplicationStartMethod(
    typeof(MasterBootstrapper),
    "StartUp")]

[assembly: WebActivator.ApplicationShutdownMethod(
    typeof(MasterBootstrapper),
    "ShutDown")]

namespace DDDIntro.Web.Infrastructure.Bootstrapping
{
    public static class MasterBootstrapper
    {
        private static IWindsorContainer container;

        public static void StartUp()
        {
            container = new WindsorContainer().Install(FromAssembly.This());

            // don't really want this but one of our bootstrappers needs it. want to avoid Service Locator anti-pattern
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            var bootStrappers = container.ResolveAll<IBootstrapper>();

            foreach (var bootStrapper in bootStrappers)
            {
                bootStrapper.StartUp();
            }
        }

        public static void ShutDown()
        {
            var bootStrappers = container.ResolveAll<IBootstrapper>();

            foreach (var bootStrapper in bootStrappers)
            {
                bootStrapper.ShutDown();
            }

            container.Dispose();
        }        
    }
}