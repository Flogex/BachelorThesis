using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Flogex.Thesis.NeverNote.Shared.Logging
{
    public class RequestNoEnricherMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestNoEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue("Request-No", out var requestNo);

            using (LogContext.PushProperty("RequestNo", requestNo))
            {
                await _next(context);
            }
        }
    }
}
