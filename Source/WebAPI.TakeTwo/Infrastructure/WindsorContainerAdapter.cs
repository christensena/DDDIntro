using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ServiceStack.Configuration;

namespace DDDIntro.WebAPI.TakeTwo.Infrastructure
{
    public class WindsorContainerAdapter : IContainerAdapter, IRelease, IDisposable
    {
        private readonly IWindsorContainer container;

        private WindsorContainerAdapter(IWindsorContainer container)
        {
            this.container = container;
        }

        public static WindsorContainerAdapter Create()
        {
            var windsorContainer = new WindsorContainer();
            windsorContainer.Install(FromAssembly.This());

            return new WindsorContainerAdapter(windsorContainer);
        }

        public T TryResolve<T>()
        {
            if (!container.Kernel.HasComponent(typeof(T)))
            {
                return default(T);
            }

            return container.Resolve<T>();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public void Release(object instance)
        {
            container.Release(instance);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}