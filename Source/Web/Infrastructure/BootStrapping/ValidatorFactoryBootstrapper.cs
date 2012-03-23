using FluentValidation.Mvc;

namespace DDDIntro.Web.Infrastructure.Bootstrapping
{
    public class ValidatorFactoryBootstrapper : IBootstrapper
    {
        private readonly WindsorValidatorFactory validatorFactory;

        public ValidatorFactoryBootstrapper(WindsorValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public void StartUp()
        {
            FluentValidationModelValidatorProvider.Configure(provider =>
                                                                 {
                                                                     provider.ValidatorFactory = validatorFactory;
                                                                 });
        }

        public void ShutDown()
        {
        }
    }
}