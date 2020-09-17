using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Flogex.Thesis.NeverNote.Shared.Logging
{
    public static class Logger
    {
        public static ILogger GetDefault()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
