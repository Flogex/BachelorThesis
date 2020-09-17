using Flogex.Thesis.NeverNote.Rest.Authors;
using Flogex.Thesis.NeverNote.Rest.Keywords;
using Flogex.Thesis.NeverNote.Rest.Notes;
using Flogex.Thesis.NeverNote.Shared.Authentication;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Logging;
using JsonApiDotNetCore;
using JsonApiDotNetCore.Models.JsonApiDocuments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flogex.Thesis.NeverNote.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataRepositories();
            services.AddResourceService<AuthorsService>();
            services.AddResourceService<NotesService>();
            services.AddResourceService<KeywordsService>();

            var mvcBuilder = services.AddMvcCore();

            services.AddJsonApi(
                options: _ =>
                {
                    _.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
                    _.EnableResourceHooks = true;
                    _.UseRelativeLinks = true;
                    _.TopLevelLinks = Links.Self | Links.Related;
                },
                mvcBuilder: mvcBuilder,
                discovery: _ => _.AddCurrentAssembly());

            services.AddAuthentication("Basic").AddBasic();

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

            app.UseJsonApi();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
