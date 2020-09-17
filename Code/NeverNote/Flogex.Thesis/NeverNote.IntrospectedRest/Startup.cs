using Flogex.Thesis.IntRest;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.IntRest.Introspection;
using Flogex.Thesis.IntRest.Runtime;
using Flogex.Thesis.NeverNote.Shared.Authentication;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flogex.Thesis.NeverNote.IntrospectedRest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAuthentication("Basic").AddBasic();

            services.AddDataRepositories();

            services.AddIntrospectedRest(options =>
            {
                options.Microtypes
                    .RegisterIntrospectionMicrotype(new MethodsOnlyIntrospection())
                    .RegisterIntrospectionMicrotype(new JsonHyperSchemaIntrospection(options))
                    .RegisterIntrospectionMicrotype(new LinksOnlyIntrospection(options))
                    .RegisterContentIntrospectionExecutor(new PlainJsonJsonSchemaIntrospection())
                    .RegisterContentIntrospectionExecutor(new JsonHomeJsonSchemaIntrospection())
                    .RegisterContentMicrotype(new JsonContentMicrotypeDescriptor())
                    .RegisterContentMicrotype(new JsonHomeMicrotypeDescriptor())
                    .RegisterRuntimeMicrotype(new PaginationMicrotypeDescriptor())
                    .RegisterRuntimeMicrotype(new ErrorMicrotypeDescriptor());

                options.JsonSerializerOptions.WriteIndented = false;
            });

            services.AddControllers(_ =>
            {
                _.CacheProfiles.Add("Cache60", new CacheProfile
                {
                    Duration = 60,
                    Location = ResponseCacheLocation.Any
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseIntrospection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
