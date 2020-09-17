using Microsoft.AspNetCore.Mvc.Routing;

namespace Flogex.Thesis.IntRest.Attributes
{
    public class RestPutAttribute : HttpMethodAttribute
    {
        private static readonly string[] _httpMethods = new string[]
        {
            "PUT", "OPTIONS"
        };

        public RestPutAttribute() : base(_httpMethods) { }

        public RestPutAttribute(string template) : base(_httpMethods, template) { }
    }
}
