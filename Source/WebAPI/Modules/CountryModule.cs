using System.IO;
using System.Linq;
using DDDIntro.Domain.Services.Factories;
using DDDIntro.Persistence;
using DDDIntro.WebAPI.Resources;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;

namespace DDDIntro.WebAPI.Modules
{
    public class CountryModule : NancyModule
    {
        // TODO: let's do content negotiation ourselves unless there is a nice
        // way to do it in Nancy like there seems to be for most things
        public CountryModule(IRepository<Domain.Country> countryRepository, CountryFactory countryFactory)
        {
            Get["/countries"] = parameters =>
                {
                    var countryResources = countryRepository.FindAll()
                        .Select(c => new Country {id = c.Id.ToString(), Name = c.Name});

                    var resource = countryResources.ToArray();

                    return Response.AsJson(resource);
                };

            Get["/countries/{id}"] = parameters =>
                {
                    var country = countryRepository.GetById((int)parameters.id);
                    if (country == null)
                        return HttpStatusCode.NotFound;

                    var resource = new Country {id = country.Id.ToString(), Name = country.Name};

                    return Response.AsJson(resource);
                };

            Post["/countries"] = parameters =>
                {
                    var countryResource = this.Bind<Country>(blacklistedProperties:"id");

                    var validationResult = this.Validate(countryResource);
                    if (! validationResult.IsValid)
                    {
                        var response = Response.AsJson(validationResult.Errors);
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }

                    var country = countryFactory.CreateCountry(countryResource.Name);
                    countryRepository.Add(country);

                    return Response.AsRedirect("/countries/"+country.Id);
                };

            Put["/countries/{id}"] = parameters =>
                {
                    var country = countryRepository.GetById((int) parameters.id);
                    if (country == null)
                        return HttpStatusCode.NotFound; // this correct for a put? should probably be some error

                    // we don't actually support updates to countries!
                    //country.Name = parameters.Name;

                    return Response.AsRedirect("/countries/" + country.Id);
                };

            Delete["/countries/{id}"] = parameters =>
                {
                    var country = countryRepository.GetById((int)parameters.id);
                    if (country == null)
                        return HttpStatusCode.NotFound; // this correct for a put? should probably be some error

                    countryRepository.Remove(country);

                    return HttpStatusCode.OK;
                };
        }
    }
}