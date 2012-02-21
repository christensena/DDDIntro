using System.Collections.Generic;
using AutoMapper;
using DDDIntro.Domain;
using DDDIntro.Web.ViewModels;

namespace DDDIntro.Web.Infrastructure
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Country, CountryViewModel>();
            Mapper.CreateMap<IEnumerable<Country>, CountryIndexViewModel>()
                .ForMember(m => m.Countries, m => m.MapFrom(x => x));
        }
    }
}