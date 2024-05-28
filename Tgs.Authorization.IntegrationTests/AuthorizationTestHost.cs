using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using Tgs.Authorization.Repositories.Entities;

namespace Tgs.Authorization.IntegrationTests;

internal class AuthorizationTestHost
{
    public static WebApplicationFactory<Program> Instance => new WebApplicationFactory<Program>()
        .WithWebHostBuilder(hostBuilder => hostBuilder
            .ConfigureAppConfiguration((_, config) =>
            {
                config.Sources.Clear();
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ENVIRONMENT"] = "Development",
                });
            })
            .ConfigureLogging(logging => logging.ClearProviders())
            .ConfigureServices(services => services
                .AddSingleton(sp => new TgsContext(
                    new DbContextOptionsBuilder<TgsContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options))
                .PostConfigure<OtlpExporterOptions>(options => options.TimeoutMilliseconds = 0)));
}