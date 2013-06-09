using System.Linq;
using System.Net;
using DDDIntro.Domain.Services.Factories;
using DDDIntro.Persistence;
using DDDIntro.WebAPI.TakeTwo.Resources;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DDDIntro.WebAPI.TakeTwo.RestServices
{
    public class CountryRestService : RestServiceBase<Country>
    {
        private readonly IRepository<Domain.Country> repository;
        private readonly CountryFactory countryFactory;

        public CountryRestService(IRepository<Domain.Country> repository, CountryFactory countryFactory)
        {
            this.repository = repository;
            this.countryFactory = countryFactory;
        }

        public override object OnGet(Country request)
        {
            if (request.Id == default(int))
            {
                var countryResources = repository.FindAll()
                        .Select(c => new Country {Id = c.Id, Name = c.Name});

                return countryResources.ToArray();
            }

            var country = repository.GetById(request.Id);
            if (country == null)
                throw new HttpError(HttpStatusCode.NotFound, "Country Not Found");

            return new Country {Id = country.Id, Name = country.Name};
        }

        public override object OnPost(Country request)
        {
            //
            //                    var validationResult = this.Validate(countryResource);
            //                    if (! validationResult.IsValid)
            //                    {
            //                        var response = Response.AsJson(validationResult.Errors);
            //                        response.StatusCode = HttpStatusCode.BadRequest;
            //                        return response;
            //                    }
            //
            var country = countryFactory.CreateCountry(request.Name);
            repository.Add(country);
            
            return new HttpResult
                       {
                           StatusCode = HttpStatusCode.Created,
                           Headers =
                               {
                                   {HttpHeaders.Location, RequestContext.AbsoluteUri.WithTrailingSlash() + country.Id }
                               }
                       };
        }

        public override object OnPut(Country request)
        {
            var country = repository.GetById(request.Id);
            if (country == null)
                throw new HttpError(HttpStatusCode.NotFound, "Country Not Found");

            // we don't actually support updates to countries!
            //country.Name = parameters.Name;
            return new HttpResult
            {
                StatusCode = HttpStatusCode.NoContent,
                Headers =
                               {
                                   {HttpHeaders.Location, RequestContext.AbsoluteUri.WithTrailingSlash() + country.Id }
                               }
            };
        }

        public override object OnDelete(Country request)
        {
            var country = repository.GetById(request.Id);
            if (country == null)
                throw new HttpError(HttpStatusCode.NotFound, "Country Not Found");
           
            repository.Remove(country);

            return new HttpResult
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Headers = 
                    {
                        { HttpHeaders.Location, RequestContext.AbsoluteUri.WithTrailingSlash() + request.Id }
                    }
                };
        }
    }
}