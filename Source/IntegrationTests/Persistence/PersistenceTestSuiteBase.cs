using DDDIntro.Core;
using NHibernate;
using NUnit.Framework;
using Persistence;

namespace DDDIntro.IntegrationTests.Persistence
{
    public abstract class PersistenceTestSuiteBase
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private ISessionFactory sessionFactory;
        private ISession repositorySession;

        protected IUnitOfWorkFactory UnitOfWorkFactory
        {
            get { return unitOfWorkFactory; }
        }

        protected IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new NHibernateRepository<TEntity>(repositorySession);
        }

        [TestFixtureSetUp]
        public void PersistenceTestSuiteBaseFixtureSetUp()
        {
            var databaseConfiguration = NHibernateConfigurationProvider.GetTempDatabaseConfiguration();
            sessionFactory = new SessionFactoryProvider(databaseConfiguration).GetSessionFactory();
            unitOfWorkFactory = new NHibernateUnitOfWorkFactory(sessionFactory);
        }

        [SetUp]
        public void PersistenceTestSuiteBaseSetUp()
        {
            repositorySession = sessionFactory.OpenSession();
        }

        [TearDown]
        public void PersistenceTestSuiteBaseTearDown()
        {
            repositorySession.Dispose();
        }
        
    }
}