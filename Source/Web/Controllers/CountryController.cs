﻿using System.Linq;
using System.Web.Mvc;
using DDDIntro.Core;
using DDDIntro.Domain;
using DDDIntro.Domain.Services.Factories;
using DDDIntro.Web.ViewModels;

namespace DDDIntro.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly IRepository<Country> repository;
        private readonly CountryFactory countryFactory;

        public CountryController(IRepository<Country> repository, CountryFactory countryFactory)
        {
            this.repository = repository;
            this.countryFactory = countryFactory;
        }

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

        public ActionResult Create()
        {
            return View(new CreateCountryViewModel());
        } 

        [HttpPost]
        public ActionResult Create(CreateCountryViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var country = countryFactory.CreateCountry(viewModel.Name);

            repository.Add(country);

            return RedirectToAction("Index");
        }
        
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
