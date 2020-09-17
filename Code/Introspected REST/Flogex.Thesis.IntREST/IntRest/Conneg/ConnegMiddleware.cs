using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Flogex.Thesis.IntRest.Conneg
{
    /* Parse Accept-Header
     * Check if there are registered IComponents that can handle microtypes
     * If there is a failure, return Not-Acceptable
     * Add Microtypes to HttpContext.Items
     */
    public class ConnegMiddleware
    {
        private readonly RequestDelegate _next;

        public ConnegMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var headers = context.Request.GetTypedHeaders();
            var acceptedMediaTypes = headers.Accept;

            var containerMediaType = acceptedMediaTypes.SingleOrDefault(m => m.MediaType == "application/vnd.microtype-container+json");

            if (containerMediaType != null)
            {
                var acceptableMicroTypes = ParseMediaType(containerMediaType);
                context.Items.Add(MicrotypeCollection.Identifier, acceptableMicroTypes);

                return _next(context);
            }
            else if (acceptedMediaTypes.Any(m => (m.Type == "application" || m.MatchesAllTypes) && m.MatchesAllSubTypes))
            {
                //TODO Accepts all
                return _next(context);
            }
            else
                return HandleNotAcceptable(context);
        }

        private static MicrotypeCollection ParseMediaType(MediaTypeHeaderValue mediaType)
        {
            var microTypes = new MicrotypeCollection();

            foreach (var parameter in mediaType.Parameters)
                if (parameter.Name != "q")
                    microTypes.Add(parameter.Name.ToString(), parameter.Value.ToString());

            return microTypes;
        }

        private async Task HandleNotAcceptable(HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status406NotAcceptable;
            await response.WriteAsync("Mediatype application/vnd.microtype-container+json must be accepted by client.");
            await response.CompleteAsync();
        }
    }
}
