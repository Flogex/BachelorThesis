using Microsoft.Extensions.DependencyInjection;

namespace Flogex.Thesis.NeverNote.Shared.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            return services
                .AddSingleton(_ =>
                {
                    var repo = new AuthorsRepository();
                    repo.Initialize().GetAwaiter().GetResult();
                    return repo;
                })
                .AddSingleton(services =>
                {
                    var authorRepo = services.GetRequiredService<AuthorsRepository>();
                    var repo = new NotesRepository();
                    repo.Initialize(authorRepo).GetAwaiter().GetResult();
                    return repo;
                });
        }
    }
}
