using System;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor.Installer;
using Nancy.Bootstrappers.Windsor;

namespace DDDIntro.WebAPI.Infrastructure
{
    public class NancyBootstrapper : WindsorNancyBootstrapper
    {
        private IDisposable lifestyleScope;

        protected override void ApplicationStartup(Castle.Windsor.IWindsorContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.BeforeRequest += context => {
                                                      lifestyleScope = container.BeginScope();
                                                      return null;
            };

            pipelines.AfterRequest += context => lifestyleScope.Dispose();
        }

        protected override void ConfigureApplicationContainer(Castle.Windsor.IWindsorContainer existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);

            existingContainer.Install(FromAssembly.This());
        }
    }
}