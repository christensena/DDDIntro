using System;
using System.Web.Mvc;
using DDDIntro.Core;

namespace DDDIntro.Web.Infrastructure.GlobalFilters
{
    public class SessionPerRequestActionFilter : IActionFilter
    {
        private readonly Func<IUnitOfWorkFactory> unitOfWorkFactoryProvider;
        private IUnitOfWork unitOfWork;

        public SessionPerRequestActionFilter(Func<ISessionSharingUnitOfWorkFactory> unitOfWorkFactoryProvider)
        {
            if (unitOfWorkFactoryProvider == null) throw new ArgumentNullException("unitOfWorkFactoryProvider");
            this.unitOfWorkFactoryProvider = unitOfWorkFactoryProvider;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var unitOfWorkFactory = unitOfWorkFactoryProvider.Invoke();
            unitOfWork = unitOfWorkFactory.BeginUnitOfWork();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                unitOfWork.Complete();
            }

            unitOfWork.Dispose();
        }
    }
}