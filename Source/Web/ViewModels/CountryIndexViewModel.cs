using System.Collections.Generic;

namespace DDDIntro.Web.ViewModels
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