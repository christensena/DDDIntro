using System;

namespace DDDIntro.WebAPI.Resources
{
    public class MatchResource
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }

        public int Team1CountryId { get; set; }

        public int Team2CountryId { get; set; }
    }
}