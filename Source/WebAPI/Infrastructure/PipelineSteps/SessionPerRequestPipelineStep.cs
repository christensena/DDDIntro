using System;
using Castle.Windsor;
using DDDIntro.Persistence;
using Nancy;
using Nancy.Bootstrapper;

namespace DDDIntro.WebAPI.Infrastructure.PipelineSteps
{
    public static class SessionPerRequestPipelineStep
    {
        public static void Register(IPipelines pipelines, IWindsorContainer container)
        {
            pipelines.BeforeRequest += context => BeginUnitOfWork(context, container);
            
            pipelines.OnError += RegisterThatWeHadAnError;

            pipelines.AfterRequest += EndUnitOfWork;
        }

        private static Response BeginUnitOfWork(NancyContext context, IWindsorContainer container)
        {
            context.Items["UnitOfWork"] = container.Resolve<IUnitOfWorkFactory>().BeginUnitOfWork();
            return null;
        }

        private static Response RegisterThatWeHadAnError(NancyContext context, Exception exception)
        {
            context.Items["SessionPerRequestError"] = true;
            return null;
        }

        private static void EndUnitOfWork(NancyContext context)
        {
            // todo: get to the bottom of why this guard condition is needed with static resources
            if (!context.Items.ContainsKey("UnitOfWork")) return;

            var unitOfWork = context.Items["UnitOfWork"] as IUnitOfWork;

            try
            {
                if (! DidWeHaveAnError(context))
                {
                    unitOfWork.Complete();
                }
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        private static bool DidWeHaveAnError(NancyContext context)
        {
            return context.Items.ContainsKey("SessionPerRequestError") && (bool)context.Items["SessionPerRequestError"];
        }
    }
}