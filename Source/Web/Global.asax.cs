using System.Web.Mvc;

namespace DDDIntro.Web
{
    // see Infrastructure/BootStrapping for what is normally in here
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}