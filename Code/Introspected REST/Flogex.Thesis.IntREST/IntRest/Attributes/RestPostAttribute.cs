using Microsoft.AspNetCore.Mvc.Routing;

namespace Flogex.Thesis.IntRest.Attributes
{
    public class RestPostAttribute : HttpMethodAttribute
    {
        private static readonly string[] _httpMethods = new string[]
        {
            "POST", "OPTIONS"
        };

        public RestPostAttribute() : base(_httpMethods) { }

        public RestPostAttribute(string template) : base(_httpMethods, template) { }
    }
}
