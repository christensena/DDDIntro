using Nancy;

namespace DDDIntro.WebAPI.Modules
{
    public class HomeModule : NancyModule 
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
                {
                    return View["Index"];
                };

        }
    }
}