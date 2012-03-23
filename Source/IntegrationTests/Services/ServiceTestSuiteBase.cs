using System;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DDDIntro.Core;
using NUnit.Framework;

namespace DDDIntro.IntegrationTests.Services
{
    public class ServiceTestSuiteBase
    {
        private static readonly IWindsorContainer Container = new WindsorContainer().Install(FromAssembly.This());
        private IDisposable lifeStyleScope;

        protected IUnitOfWorkFactory UnitOfWorkFactory
        {
            get { return Resolve<IUnitOfWorkFactory>(); }
        }

        protected TComponent Resolve<TComponent>()
        {
            return Container.Resolve<TComponent>();
        }

        [SetUp]
        public void ServiceTestSuiteBaseSetUp()
        {
            lifeStyleScope = Container.BeginScope();
        }

        [TearDown]
        public void ServiceTestSuiteBaseTearDown()
        {
            lifeStyleScope.Dispose();
        }
    }
}