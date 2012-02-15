using System;
using System.Web.Mvc;
using NHibernate;

namespace DDDIntro.Web.Infrastructure.GlobalFilters
{
    public class SessionPerRequestActionFilter : IActionFilter
    {
        private readonly Func<ISession> sessionProvider;
        private ISession session;
        private ITransaction transaction;

        public SessionPerRequestActionFilter(Func<ISession> sessionProvider)
        {
            if (sessionProvider == null) throw new ArgumentNullException("sessionProvider");
            this.sessionProvider = sessionProvider;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            session = sessionProvider.Invoke();
            transaction = session.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (transaction.IsActive)
                    transaction.Commit();
            }
            finally
            {
                transaction.Dispose();
                session.Dispose();
            }
        }
    }
}