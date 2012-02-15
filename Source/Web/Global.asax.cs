using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DDDIntro.Web.Infrastructure;
using DDDIntro.Web.Infrastructure.GlobalFilters;
using NHibernate;

namespace DDDIntro.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionPerRequestActionFilter(() => container.Resolve<ISession>()));
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            container = BootstrapContainer();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            container.Dispose();
        }

        private static IWindsorContainer container;

        private static IWindsorContainer BootstrapContainer()
        {
            var windsorContainer = new WindsorContainer().Install(FromAssembly.This());

            var controllerFactory = new WindsorControllerFactory(windsorContainer.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            return windsorContainer;
        }
    }
}