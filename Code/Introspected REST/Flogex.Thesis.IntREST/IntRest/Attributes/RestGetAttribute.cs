using Microsoft.AspNetCore.Mvc.Routing;

namespace Flogex.Thesis.IntRest.Attributes
{
    public class RestGetAttribute : HttpMethodAttribute
    {
        private static readonly string[] _httpMethods = new string[]
        {
            "GET", "OPTIONS"
        };


        public RestGetAttribute() : base(_httpMethods) { }

        public RestGetAttribute(string template) : base(_httpMethods, template) { }
    }
}
