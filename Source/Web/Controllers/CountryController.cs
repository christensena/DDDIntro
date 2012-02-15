using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                viewModel.Countries.Add(new CountryViewModel {Id = country.Id, Name = country.Name});
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
            try
            {
                repository.Add(new Country(viewModel.Name));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Country/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Country/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Country/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Country/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
