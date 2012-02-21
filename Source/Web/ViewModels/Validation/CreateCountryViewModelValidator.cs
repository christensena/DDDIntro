using System.Linq;
using DDDIntro.Core;
using DDDIntro.Domain;
using FluentValidation;

namespace DDDIntro.Web.ViewModels.Validation
{
    /// <summary>
    /// So we do our validation here. 
    /// Invariants will be rudely enforced (exceptions) if we let nonsense through
    /// so we need to do validation.
    /// </summary>
    /// <remarks>
    /// For those obsessed with DRY. Remember we are validating only the view model, not the whole domain model.
    /// Stop thinking CRUD!
    /// </remarks>
    public class CreateCountryViewModelValidator : AbstractValidator<CreateCountryViewModel>
    {
        private readonly IRepository<Country> repository;

        public CreateCountryViewModelValidator(IRepository<Country> repository)
        {
            this.repository = repository;
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Must(BeUnique).WithMessage("Name must be unique.");
        }

        // instead of a repository we could use specifications
        // e.g. new CountryNameNotAlreadyTakenSpecification().IsSatisfied(name)
        // the specification object would do the lookup
        private bool BeUnique(string candidateName)
        {
            return !repository.FindAll().Any(c => c.Name == candidateName);
        }
    }
}