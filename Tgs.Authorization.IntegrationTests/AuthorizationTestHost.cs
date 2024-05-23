using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            .ConfigureServices(services => services
                .AddSingleton(sp => new TgsContext(
                    new DbContextOptionsBuilder<TgsContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options))));
}