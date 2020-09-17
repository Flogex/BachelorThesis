using Flogex.Thesis.IntRest.Conneg;
using Flogex.Thesis.IntRest.Introspection;
using Microsoft.AspNetCore.Builder;

namespace Flogex.Thesis.IntRest
{
    public static class PipelineBuilding
    {
        public static IApplicationBuilder UseIntrospection(this IApplicationBuilder app)
        {
            //TODO Check IIntrospectionExecutor service registered

            return app
                .UseMiddleware<ConnegMiddleware>()
                .UseMiddleware<IntrospectionMiddleware>();
        }
    }
}
