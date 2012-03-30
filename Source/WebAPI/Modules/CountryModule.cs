using System.Linq;
using DDDIntro.Persistence;
using DDDIntro.WebAPI.Resources;
using Nancy;

namespace DDDIntro.WebAPI.Modules
{
    public class CountryModule : NancyModule
    {
        // TODO: let's do content negotiation ourselves unless there is a nice
        // way to do it in Nancy like there seems to be for most things
        public CountryModule(IRepository<Domain.Country> countryRepository)
        {
            Get["/countries"] = parameters =>
                {
                    var countryResources = countryRepository.FindAll()
                        .Select(c => new Country {Id = c.Id, Name = c.Name});

                    var resource = new Countries {List = countryResources.ToList()};

                    return Response.AsJson(resource);
                };

            Get["/country/{Id}"] = parameters =>
                {
                    var country = countryRepository.GetById((int)parameters.Id);
                    if (country == null)
                        return HttpStatusCode.NotFound;

                    var resource = new Country {Id = country.Id, Name = country.Name};

                    return Response.AsJson(resource);
                };
        }
    }
}