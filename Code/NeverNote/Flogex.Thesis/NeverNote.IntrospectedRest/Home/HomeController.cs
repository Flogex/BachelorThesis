using Flogex.Thesis.IntRest;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Content;
using Microsoft.AspNetCore.Mvc;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Home
{
    [Route("/")]
    public class HomeController : Controller
    {
        [SupportsContentMicrotype("json-home")]
        [Returns(typeof(JsonHomeData))]
        public IActionResult GetHomepage()
        {
            var response = new JsonHomeData("NeverNote API");
            return new RestResult(response);
        }
    }
}
