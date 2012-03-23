using System;
using Castle.MicroKernel;
using Castle.Windsor;
using FluentValidation;

namespace DDDIntro.Web.Infrastructure
{
    public class WindsorValidatorFactory : ValidatorFactoryBase
    {
        private readonly IWindsorContainer container;

        public WindsorValidatorFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (!container.Kernel.HasComponent(validatorType))
                return null;

            return container.Resolve(validatorType) as IValidator;
        }
    }
}