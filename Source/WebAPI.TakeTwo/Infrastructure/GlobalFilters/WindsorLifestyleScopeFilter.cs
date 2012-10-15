using System;
using System.Collections.Generic;
using System.Web;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.Windsor;
using Funq;
using ServiceStack.ServiceHost;

namespace DDDIntro.WebAPI.TakeTwo.Infrastructure.GlobalFilters
{
    public class WindsorLifestyleScopeFilter
    {
        private readonly Container container;

        public WindsorLifestyleScopeFilter(Container container)
        {
            this.container = container;
        }

        public void ConfigureRequestFilters(List<Action<IHttpRequest, IHttpResponse, object>> requestFilters)
        {
            requestFilters.Add((req, res, obj) =>
                                   {
                                       IDisposable lifestyleScope = CallContextLifetimeScope.ObtainCurrentScope();
                                       if (lifestyleScope == null)
                                       {
                                           var windsorContainer = container.Resolve<IWindsorContainer>();
                                           lifestyleScope = windsorContainer.BeginScope();
                                       }
                                       HttpContext.Current.Items["LifestyleScope"] = lifestyleScope;
                                   });
        }

        public void ConfigureResponseFilters(List<Action<IHttpRequest, IHttpResponse, object>> responseFilters)
        {
            responseFilters.Add((req, res, obj) =>
                                    {
                                        {
                                            var lifestyleScope =
                                                (IDisposable) HttpContext.Current.Items["LifestyleScope"];
                                            lifestyleScope.Dispose();
                                        }
                                    });
        }
    }
}