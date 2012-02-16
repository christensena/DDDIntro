using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;
using FluentValidation;

namespace DDDIntro.Web.ViewModels.Validation
{
    public class CreateCountryViewModelValidator : AbstractValidator<CreateCountryViewModel>
    {
        private readonly IRepository<Country> repository;

        public CreateCountryViewModelValidator(IRepository<Country> repository)
        {
            this.repository = repository;
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Must(BeUnique).WithMessage("Name must be unique.");
        }

        private bool BeUnique(string candidateName)
        {
            return !repository.FindAll().Any(c => c.Name == candidateName);
        }
    }
}