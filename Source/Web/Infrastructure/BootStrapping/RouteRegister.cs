using System.Web.Mvc;
using System.Web.Routing;

namespace DDDIntro.Web.Infrastructure.Bootstrapping
{
    public class RouteRegister : IBootstrapper
    {
        public void StartUp()
        {
            var routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public void ShutDown()
        {
        }
    }
}