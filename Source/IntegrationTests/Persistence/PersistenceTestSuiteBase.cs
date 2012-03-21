using DDDIntro.Core;
using DDDIntro.Domain.Abstract;
using DDDIntro.Persistence.NHibernate;
using NHibernate;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Persistence
{
    public abstract class PersistenceTestSuiteBase
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private ISessionFactory sessionFactory;
        private ISession session;

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
            var databaseConfiguration = TempDatabaseNHibernateConfigurationProvider.GetTempDatabaseConfiguration();
            sessionFactory = databaseConfiguration.BuildSessionFactory();
            session = sessionFactory.OpenSession();
            unitOfWorkFactory = new SessionSharingNHibernateUnitOfWorkFactory(session);
            TempDatabaseNHibernateConfigurationProvider.InitialiseDatabase(databaseConfiguration, session);
        }

        [TearDown]
        public void PersistenceTestSuiteBaseTearDown()
        {
            session.Dispose();
            sessionFactory.Dispose();
        }
    }
}