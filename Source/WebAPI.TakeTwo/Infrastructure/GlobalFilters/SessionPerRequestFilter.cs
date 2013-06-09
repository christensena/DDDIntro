using System;
using System.Collections.Generic;
using System.Web;
using DDDIntro.Persistence;
using Funq;
using ServiceStack.ServiceHost;

namespace DDDIntro.WebAPI.TakeTwo.Infrastructure.GlobalFilters
{
    public class SessionPerRequestFilter
    {
        private readonly Container container;

        public SessionPerRequestFilter(Container container)
        {
            this.container = container;
        }

        public void ConfigureRequestFilters(
            List<Action<IHttpRequest, IHttpResponse, object>> requestFilters)
        {
            requestFilters.Add((req, res, obj) =>
                                   {
                                       var unitOfWorkFactory = container.Resolve<IUnitOfWorkFactory>();
                                       var unitOfWork = unitOfWorkFactory.BeginUnitOfWork();
                                       HttpContext.Current.Items["UnitOfWork"] = unitOfWork;
                                   });
        }

        public void ConfigureResponseFilters(List<Action<IHttpRequest, IHttpResponse, object>> responseFilters)
        {
            responseFilters.Add((req, res, obj) =>
                                    {
                                        var unitOfWork = (IUnitOfWork) HttpContext.Current.Items["UnitOfWork"];

                                        try
                                        {
                                            if (HttpContext.Current.Error == null)
                                            {
                                                unitOfWork.Complete();
                                            }
                                        }
                                        finally
                                        {
                                            unitOfWork.Dispose();
                                        }
                                    });
        }
    }
}