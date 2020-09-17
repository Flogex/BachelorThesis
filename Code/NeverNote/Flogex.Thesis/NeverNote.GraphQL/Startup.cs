using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.GraphQL.Serialization;
using Flogex.Thesis.NeverNote.GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Authentication;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Logging;
using GraphQL.Server;
using GraphQL.Server.Ui.Altair;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flogex.Thesis.NeverNote.GraphQL
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
            services.AddAuthentication("Basic").AddBasic();

            services.AddDataRepositories();

            services.AddSingleton<NeverNoteSchema>();
            services
                .AddGraphQL(_ =>
                {
                    _.ExposeExceptions = true;
                    _.EnableMetrics = false;
                })
                .AddGraphTypes()
                .AddSystemTextJson(configureSerializerSettings: JsonSerializationOptions.Value)
                .AddUserContextBuilder(httpContext => Task.FromResult(new GraphQlUserContext
                {
                    User = httpContext.User
                }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestNoEnricherMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMiddleware<ServerTimingMiddleware>();

            app.UseGraphQL<NeverNoteSchema>("/graphql");
            app.UseGraphiQLServer(new GraphiQLOptions { Path = "/ui/graphiql", GraphQLEndPoint = "/graphql" });
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { Path = "/ui/playground", GraphQLEndPoint = "/graphql" });
            app.UseGraphQLAltair(new GraphQLAltairOptions { Path = "/ui/altair", GraphQLEndPoint = "/graphql" });
        }
    }
}
