using System;
using Castle.Windsor;
using FluentValidation;
using Nancy.Validation;
using Nancy.Validation.FluentValidation;

namespace DDDIntro.WebAPI.Infrastructure
{
    public class WindsorFluentValidationValidatorFactory : IModelValidatorFactory
    {
        private readonly IWindsorContainer container;
        private readonly IFluentAdapterFactory fluentAdapterFactory;

        public WindsorFluentValidationValidatorFactory(IWindsorContainer container, IFluentAdapterFactory fluentAdapterFactory)
        {
            this.container = container;
            this.fluentAdapterFactory = fluentAdapterFactory;
        }

        public IModelValidator Create(Type validatorType)
        {
            if (!container.Kernel.HasComponent(validatorType))
                return null;

            var validator = container.Resolve(validatorType) as IValidator;

            return new FluentValidationValidator(validator, fluentAdapterFactory);
        }
    }
}