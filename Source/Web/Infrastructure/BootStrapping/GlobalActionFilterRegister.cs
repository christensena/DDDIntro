using System.Web.Mvc;
using Castle.Windsor;
using DDDIntro.Persistence;
using DDDIntro.Web.Infrastructure.GlobalFilters;

namespace DDDIntro.Web.Infrastructure.Bootstrapping
{
    public class GlobalActionFilterRegister : IBootstrapper
    {
        private readonly IWindsorContainer container;

        public GlobalActionFilterRegister(IWindsorContainer container)
        {
            this.container = container;
        }

        public void StartUp()
        {
            var filters = System.Web.Mvc.GlobalFilters.Filters;
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionPerRequestActionFilter(() => container.Resolve<IUnitOfWorkFactory>()));
        }

        public void ShutDown()
        {
        }
    }
}