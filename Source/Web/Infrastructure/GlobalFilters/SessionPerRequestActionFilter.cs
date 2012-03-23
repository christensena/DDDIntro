using System;
using System.Web.Mvc;
using DDDIntro.Core;

namespace DDDIntro.Web.Infrastructure.GlobalFilters
{
    public class SessionPerRequestActionFilter : IActionFilter
    {
        private readonly Func<IUnitOfWorkFactory> unitOfWorkFactoryProvider;
        private IUnitOfWork unitOfWork;

        public SessionPerRequestActionFilter(Func<IUnitOfWorkFactory> unitOfWorkFactoryProvider)
        {
            if (unitOfWorkFactoryProvider == null) throw new ArgumentNullException("unitOfWorkFactoryProvider");
            this.unitOfWorkFactoryProvider = unitOfWorkFactoryProvider;
        }

        // begin the unit of work before each action
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var unitOfWorkFactory = unitOfWorkFactoryProvider.Invoke();
            unitOfWork = unitOfWorkFactory.BeginUnitOfWork();
        }

        // end the unit of work after each action
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // only complete if we didn't get an error
            if (filterContext.Exception == null)
            {
                unitOfWork.Complete();
            }

            unitOfWork.Dispose();
        }
    }
}