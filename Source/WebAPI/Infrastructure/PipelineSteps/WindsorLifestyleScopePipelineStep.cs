using System;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Nancy;
using Nancy.Bootstrapper;

namespace DDDIntro.WebAPI.Infrastructure.PipelineSteps
{
    public static class WindsorLifestyleScopePipelineStep
    {
        public static void Register(IPipelines pipelines, IWindsorContainer container)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(context => BeginScope(context, container));
            pipelines.AfterRequest.AddItemToEndOfPipeline(EndScope);
        }

        private static void EndScope(NancyContext context)
        {
            // todo: get to the bottom of why this guard condition is needed with static resources
            if (!context.Items.ContainsKey("LifestyleScope")) return;

            var lifestyleScope = context.Items["LifestyleScope"] as IDisposable;

            lifestyleScope.Dispose();
        }

        private static Response BeginScope(NancyContext context, IWindsorContainer container)
        {
            context.Items["LifestyleScope"] = container.BeginScope();
            return null;
        }
    }
}