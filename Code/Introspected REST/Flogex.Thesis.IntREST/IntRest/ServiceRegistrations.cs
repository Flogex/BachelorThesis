using System;
using Flogex.Thesis.IntRest.Configuration;
using Flogex.Thesis.IntRest.Introspection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Flogex.Thesis.IntRest
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddIntrospectedRest(
            this IServiceCollection services,
            Action<RestOptions>? restOptionsSettings = null)
        {
            var options = new RestOptions();
            restOptionsSettings?.Invoke(options);

            return services
                .AddSingleton(options)
                .RegisterDefaultServices();
        }

        private static IServiceCollection RegisterDefaultServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IActionResultExecutor<RestResult>, RestResultExecutor>()
                .AddTransient<IntrospectionChooser>();
        }

    }
}
