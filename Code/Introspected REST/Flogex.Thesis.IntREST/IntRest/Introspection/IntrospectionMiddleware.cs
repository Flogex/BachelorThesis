using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class IntrospectionMiddleware
    {
        private readonly RequestDelegate _next;

        public IntrospectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint is null)
                // throw new InvalidOperationException($"UseRouting must be called before using {nameof(IntrospectionMiddleware)}.");
                return _next(context);

            var request = context.Request;
            var action = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (request.Method != HttpMethods.Options || action is null)
                return _next(context);

            var chooser = context.RequestServices.GetRequiredService<IntrospectionChooser>();
            return chooser.ExecuteAsync(action, context);
        }
    }
}
