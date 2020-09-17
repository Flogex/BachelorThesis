using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using SerilogTimings.Extensions;

namespace Flogex.Thesis.NeverNote.Shared.Logging
{
    public class ServerTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ServerTimingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (_logger.TimeOperation("Processing in API layer"))
            {
                await _next(context);
            }
        }
    }
}
