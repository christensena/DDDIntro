using DDDIntro.Core;
using DDDIntro.Domain.Abstract;
using DDDIntro.Persistence.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    public abstract class PersistenceTestSuiteBase
    {
        private static readonly ISessionFactory SessionFactory;
        private static readonly Configuration DatabaseConfiguration;

        private IUnitOfWorkFactory unitOfWorkFactory;
        private ISession session;

        static PersistenceTestSuiteBase()
        {
            DatabaseConfiguration = TempDatabaseNHibernateConfigurationProvider.GetTempDatabaseConfiguration();
            SessionFactory = DatabaseConfiguration.BuildSessionFactory();
        }

        protected IUnitOfWorkFactory UnitOfWorkFactory
        {
            get { return unitOfWorkFactory; }
        }

        protected IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IAggregateRoot
        {
            return new NHibernateRepository<TEntity>(session);
        }
        
        [SetUp]
        public void PersistenceTestSuiteBaseSetUp()
        {
            session = SessionFactory.OpenSession();
            unitOfWorkFactory = new SessionSharingNHibernateUnitOfWorkFactory(session);
            TempDatabaseNHibernateConfigurationProvider.InitialiseDatabase(DatabaseConfiguration, session);
        }

        [TearDown]
        public void PersistenceTestSuiteBaseTearDown()
        {
            session.Dispose();
        }
    }
}