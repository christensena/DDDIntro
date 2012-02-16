using System.Web.Mvc;
using DDDIntro.Application.Services;

namespace DDDIntro.Web.Controllers
{
    public class TestDataController : Controller
    {
        private readonly TestDataGenerator testDataGenerator;

        public TestDataController(TestDataGenerator testDataGenerator)
        {
            this.testDataGenerator = testDataGenerator;
        }

        [HttpPost]
        public ActionResult Generate()
        {
            testDataGenerator.GenerateTestData();

            return RedirectToAction("Index", "Home");
        }
    }
}
