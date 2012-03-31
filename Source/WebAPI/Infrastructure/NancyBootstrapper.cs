using System;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor.Installer;
using DDDIntro.Persistence;
using Nancy.Bootstrappers.Windsor;
using Nancy.Conventions;

namespace DDDIntro.WebAPI.Infrastructure
{
    public class NancyBootstrapper : WindsorNancyBootstrapper
    {
        protected override void ApplicationStartup(Castle.Windsor.IWindsorContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // todo: put these out into some sort of SessionPerRequestPipelineStep class
            pipelines.BeforeRequest += context => {
                context.Items["LifestyleScope"] = container.BeginScope();
                context.Items["UnitOfWork"] = container.Resolve<IUnitOfWorkFactory>().BeginUnitOfWork();
                return null;
            };

            pipelines.AfterRequest += context =>
                                          {
                                              // todo: get to the bottom of why this guard condition is needed with static resources
                                              if (! context.Items.ContainsKey("LifestyleScope")) return; 

                                              var lifestyleScope = context.Items["LifestyleScope"] as IDisposable;
                                              var unitOfWork = context.Items["UnitOfWork"] as IUnitOfWork;

                                              try
                                              {
                                                  // todo: don't complete if an error; how to check for this?
                                                  unitOfWork.Complete();
                                              }
                                              finally
                                              {
                                                  unitOfWork.Dispose();
                                                  lifestyleScope.Dispose();
                                              }
                                          };
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Assets"));
        }

        protected override void ConfigureApplicationContainer(Castle.Windsor.IWindsorContainer existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);

            existingContainer.Install(FromAssembly.This());
        }
    }
}