using System.Linq;
using DDDIntro.Persistence;
using FluentValidation;

namespace DDDIntro.WebAPI.Resources.Validators
{
    /// <summary>
    /// So we do our validation for creating a country here. 
    /// Invariants will be rudely enforced (exceptions) if we let nonsense through
    /// so we need to do validation.
    /// </summary>
    /// <remarks>
    /// For those obsessed with DRY. Remember we are validating only the view model, not the whole entity.
    /// Stop thinking CRUD!
    /// </remarks>
    public class CountryValidator : AbstractValidator<Country>
    {
        private readonly IRepository<Domain.Country> repository;

        // TODO: dummed down because of issue with Nancy.FluentValidation and Nancy.Bootstrappers.Windsor
        public CountryValidator()//(IRepository<Domain.Country> repository)
        {
            //this.repository = repository;
            RuleFor(x => x.Name).NotEmpty();
//            RuleFor(x => x.Name).Must(BeUnique).WithMessage("Name must be unique.");
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