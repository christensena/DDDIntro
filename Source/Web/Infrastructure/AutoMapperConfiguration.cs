using System.Collections.Generic;
using AutoMapper;
using DDDIntro.Domain;
using DDDIntro.Web.ViewModels.Country;

namespace DDDIntro.Web.Infrastructure
{
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for our viewmodels.
        /// </summary>
        /// <remarks>
        /// Should be called once; on application start-up.
        /// These mappings are verified by a single unit test: what is registered should work!
        /// </remarks>
        public static void Configure()
        {
            Mapper.CreateMap<Country, CountryViewModel>();
            Mapper.CreateMap<IEnumerable<Country>, CountryIndexViewModel>()
                .ForMember(m => m.Countries, m => m.MapFrom(x => x));
        }
    }
}