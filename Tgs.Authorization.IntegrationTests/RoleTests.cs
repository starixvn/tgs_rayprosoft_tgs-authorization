using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tgs.Authorization.Clients;
using Tgs.Authorization.Repositories.Entities;
using Xunit.Abstractions;

namespace Tgs.Authorization.IntegrationTests;

public class RoleTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task 整合測試_建立角色()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var system = "Test";
        var roleName = "Group1";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Acta
        var createdId = await sut.CreateRoleAsync(system, roleName);
        var actual = await dbContext.RoleRows.FindAsync([createdId]);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(system, actual.System);
        Assert.Equal(roleName, actual.Name);
    }

    [Fact]
    public async Task 整合測試_刪除角色()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var system = "Test";
        var roleId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.RoleRows.AddRangeAsync(
            new RoleRow
            {
                Id = roleId,
                Name = "Group1",
                System = system
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.DeleteRoleAsync(roleId);
        var actual = await dbContext.RoleRows.FindAsync([roleId]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task 整合測試_列出系統角色列表()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var system = "Test";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.RoleRows.AddRangeAsync(
            new RoleRow
            {
                Id = Guid.NewGuid(),
                Name = "Group1",
                System = system
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.GetRolesBySystemAsync(system).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }
}