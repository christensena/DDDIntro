using System.Linq;
using DDDIntro.Domain;
using DDDIntro.Persistence;
using DDDIntro.WebAPI.Resources;
using Nancy;
using Nancy.ModelBinding;
using Country = DDDIntro.Domain.Country;

namespace DDDIntro.WebAPI.Modules
{
    public class MatchModule :NancyModule
    {
        public MatchModule(IRepository<Match> matchRepository, IRepository<Country> countryRepository)
            : base("/matches")
        {
            Get["/"] = parameters =>
                           {
                               var matches = matchRepository.FindAll();

                               var resource = matches.ToArray().Select(
                                   m => new MatchResource
                                            {
                                                Id = m.Id,
                                                Date = m.Date,
                                                Team1CountryId = m.Team1.Country.Id,
                                                Team2CountryId = m.Team2.Country.Id
                                            });

                               return Response.AsJson(resource);
                           };

            Post["/"] = parameters =>
                {
                    var resource = this.Bind<MatchResource>();

                    var country1 = countryRepository.GetById(resource.Team1CountryId);
                    if (country1 == null)
                        return HttpStatusCode.BadRequest;

                    var country2 = countryRepository.GetById(resource.Team2CountryId);
                    if (country2 == null)
                        return HttpStatusCode.BadRequest;

                    var match = Match.Create(
                        resource.Date,
                        country1,
                        country2);

                    matchRepository.Add(match);

                    return Response.AsRedirect("/matches/" + match.Id);
                };

            Get["/{id}"] = parameters =>
                {
                    var match = matchRepository.GetById((int)parameters.Id);
                    if (match == null)
                        return HttpStatusCode.NotFound;

                    return Response.AsJson(new
                        {
                            Date = match.Date,
                            Team1 = match.Team1,
                            Team2 = match.Team2
                        });
                };
        }
    }
}