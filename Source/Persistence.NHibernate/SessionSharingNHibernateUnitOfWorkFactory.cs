using System;
using DDDIntro.Core;
using NHibernate;

namespace DDDIntro.Persistence.NHibernate
{
    public class SessionSharingNHibernateUnitOfWorkFactory : ISessionSharingUnitOfWorkFactory
    {
        private readonly ISession session;

        public SessionSharingNHibernateUnitOfWorkFactory(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.session = session;
        }

        public IUnitOfWork BeginUnitOfWork()
        {
            return new NHibernateUnitOfWork(session);
        }
    }
}