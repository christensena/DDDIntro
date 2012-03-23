using System.Web.Mvc;

namespace DDDIntro.Web.Infrastructure.Bootstrapping
{
    public class ControllerFactoryBootstrapper : IBootstrapper
    {
        private readonly WindsorControllerFactory controllerFactory;

        public ControllerFactoryBootstrapper(WindsorControllerFactory controllerFactory)
        {
            this.controllerFactory = controllerFactory;
        }

        public void StartUp()
        {
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public void ShutDown()
        {
        }
    }
}