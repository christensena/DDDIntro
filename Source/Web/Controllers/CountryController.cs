using System.Linq;
using System.Web.Mvc;
using DDDIntro.Core;
using DDDIntro.Domain;
using DDDIntro.Web.ViewModels;

namespace DDDIntro.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly IRepository<Country> repository;

        public CountryController(IRepository<Country> repository)
        {
            this.repository = repository;
        }

        //
        // GET: /Country/

        public ActionResult Index()
        {
            var viewModel = new CountryIndexViewModel();

            var countries = repository.FindAll().ToArray();
            // TODO: use automapper here
            foreach (var country in countries)
            {
                var countryViewModel = new CountryViewModel {Id = country.Id, Name = country.Name};
                viewModel.Countries.Add(countryViewModel);
            }

            return View(viewModel);
        }


        //
        // GET: /Country/Create

        public ActionResult Create()
        {
            return View(new CreateCountryViewModel());
        } 

        //
        // POST: /Country/Create

        [HttpPost]
        public ActionResult Create(CreateCountryViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            repository.Add(new Country(viewModel.Name));

            return RedirectToAction("Index");
        }
        
        //
        // POST: /Country/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var country = repository.GetById(id);
            if (country == null)
                return HttpNotFound();

            repository.Remove(country);
 
            return RedirectToAction("Index");
        }
    }
}
