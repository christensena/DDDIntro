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
        private ISession repositorySession;

        protected IUnitOfWorkFactory UnitOfWorkFactory
        {
            get { return unitOfWorkFactory; }
        }

        protected IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IAggregateRoot
        {
            return new NHibernateRepository<TEntity>(repositorySession);
        }

        [SetUp]
        public void PersistenceTestSuiteBaseSetUp()
        {
            var databaseConfiguration = TempDatabaseNHibernateConfigurationProvider.GetTempDatabaseConfiguration();
            sessionFactory = databaseConfiguration.BuildSessionFactory();
            unitOfWorkFactory = new NHibernateUnitOfWorkFactory(sessionFactory);
            repositorySession = sessionFactory.OpenSession();
        }

        [TearDown]
        public void PersistenceTestSuiteBaseTearDown()
        {
            repositorySession.Dispose();
            sessionFactory.Dispose();
        }
    }
}