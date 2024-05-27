using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tgs.Authorization.Clients;
using Tgs.Authorization.Repositories.Entities;
using Xunit.Abstractions;

namespace Tgs.Authorization.IntegrationTests;

public class AuthorizeTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task 整合測試_檢查Account是否有權限()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var accountId = Guid.NewGuid();
        var functionId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.GrantRows.AddRangeAsync(
            new GrantRow
            {
                AccountId = accountId,
                FunctionId = functionId,
                Type = nameof(PermissionType.Allow)
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.AuthorizeAsync(accountId, functionId);

        // Assert
        Assert.True(actual);
    }
}