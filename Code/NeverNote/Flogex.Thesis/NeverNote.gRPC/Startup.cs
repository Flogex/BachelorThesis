using System;
using System.Net;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.gRPC;
using Flogex.Thesis.NeverNote.gRPC.Handler;
using Flogex.Thesis.NeverNote.Shared.Authentication;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NeverNote.gRPC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataRepositories();
            services.RegisterRequestHandler();
            services.AddAuthentication("Basic").AddBasic();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestNoEnricherMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseMiddleware<ServerTimingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<NeverNoteServiceImpl>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
