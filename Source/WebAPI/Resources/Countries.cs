using System.Collections.Generic;

namespace DDDIntro.WebAPI.Resources
{
    public class Countries
    {
        public IList<Country> List { get; set; }

        public Countries()
        {
            List = new List<Country>();
        }
    }
}