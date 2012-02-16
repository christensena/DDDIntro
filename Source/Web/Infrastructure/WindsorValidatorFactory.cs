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
            try
            {
                return container.Resolve(validatorType) as IValidator;
            }
            // yuck. may have to try another IoC container if can't conditionally resolve
            // without exceptions
            catch (ComponentNotFoundException)
            {
                return null;
            }
        }
    }
}