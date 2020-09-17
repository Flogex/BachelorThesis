using Microsoft.AspNetCore.Mvc.Routing;

namespace Flogex.Thesis.IntRest.Attributes
{
    public class RestDeleteAttribute : HttpMethodAttribute
    {
        private static readonly string[] _httpMethods = new string[]
        {
            "DELETE", "OPTIONS"
        };

        public RestDeleteAttribute() : base(_httpMethods) { }

        public RestDeleteAttribute(string template) : base(_httpMethods, template) { }
    }
}
