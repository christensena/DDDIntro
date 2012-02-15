using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDDIntro.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
