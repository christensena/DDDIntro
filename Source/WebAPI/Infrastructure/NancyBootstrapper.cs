using Castle.Windsor;
using Castle.Windsor.Installer;
using DDDIntro.WebAPI.Infrastructure.PipelineSteps;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Windsor;
using Nancy.Conventions;

namespace DDDIntro.WebAPI.Infrastructure
{
    public class NancyBootstrapper : WindsorNancyBootstrapper
    {
        protected override void ApplicationStartup(IWindsorContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            SessionPerRequestPipelineStep.Register(pipelines, container);

            WindsorLifestyleScopePipelineStep.Register(pipelines, container);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Assets"));
        }

        protected override void ConfigureApplicationContainer(IWindsorContainer existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);

            existingContainer.Install(FromAssembly.This());
        }
    }
}