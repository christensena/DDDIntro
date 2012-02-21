using System.Collections.Generic;

namespace DDDIntro.Web.ViewModels.Country
{
    public class CountryIndexViewModel
    {
        public List<CountryViewModel> Countries { get; set; }

        public CountryIndexViewModel()
        {
            Countries = new List<CountryViewModel>();
        }
    }
}