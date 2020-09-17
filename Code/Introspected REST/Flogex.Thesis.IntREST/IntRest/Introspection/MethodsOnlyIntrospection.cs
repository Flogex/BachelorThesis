using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class MethodsOnlyIntrospection : IIntrospectionMicrotype
    {
        public string Identifier { get; } = "methods-only";

        public Task ExecuteAsync(ControllerActionDescriptor action, HttpContext context)
        {
            var allowedHttpMethods = action.MethodInfo.GetCustomAttributes<HttpMethodAttribute>()
                .SelectMany(attr => attr.HttpMethods)
                .Select(m => m.ToUpperInvariant())
                .Distinct(StringComparer.Ordinal)
                .OrderBy(GetHttpMethodPriority);

            var response = context.Response;
            response.StatusCode = StatusCodes.Status204NoContent;
            response.Headers.Add("Allow", string.Join(", ", allowedHttpMethods));

            return context.Response.CompleteAsync();
        }

        private static int GetHttpMethodPriority(string method)
            => method switch
            {
                "OPTIONS" => 0,
                "HEAD" => 1,
                "GET" => 2,
                "POST" => 3,
                "PUT" => 4,
                "PATCH" => 5,
                "DELETE" => 6,
                _ => 7
            };
    }
}
