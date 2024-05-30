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

    [Fact]
    public async Task 整合測試_將Account指定到角色()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper))
                .ConfigureServices(services => services
                    .AddHttpClientLoggingHandler()));

        var client = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<AuthorizationClient>());

        var sut = new AuthorizationClient(client);

        var roleId = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Act
        await sut.AddAccountInRoleAsync(accountId, roleId, 100);
        var actual = await dbContext.AccountRoles.FindAsync([accountId, roleId]);

        // Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public async Task 整合測試_將Account從角色中移除()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper))
                .ConfigureServices(services => services
                    .AddHttpClientLoggingHandler()));

        var client = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<AuthorizationClient>());

        var sut = new AuthorizationClient(client);

        var roleId = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.AccountRoles
            .AddRangeAsync(new AccountRole
            {
                AccountId = accountId,
                RoleId = roleId,
                Priority = 100
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.RemoveAccountInRoleAsync(accountId, roleId);
        var actual = await dbContext.AccountRoles.FindAsync([accountId, roleId]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task 整合測試_取得指派角色的AccountId列表()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper))
                .ConfigureServices(services => services
                    .AddHttpClientLoggingHandler()));

        var client = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<AuthorizationClient>());

        var sut = new AuthorizationClient(client);

        var roleId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.AccountRoles
            .AddRangeAsync(new AccountRole
            {
                AccountId = Guid.NewGuid(),
                RoleId = roleId,
                Priority = 100
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.ListRoleAccountsAsync(roleId).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task 整合測試_為角色指派角色()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper))
                .ConfigureServices(services => services
                    .AddHttpClientLoggingHandler()));

        var client = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<AuthorizationClient>());

        var sut = new AuthorizationClient(client);

        var roleId = Guid.NewGuid();
        var parentRoleId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Act
        await sut.AddRoleInRoleAsync(roleId, parentRoleId, 100);
        var actual = await dbContext.RoleInRoles.FindAsync([roleId, parentRoleId]);

        // Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public async Task 整合測試_將角色從父角色中移除()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper))
                .ConfigureServices(services => services
                    .AddHttpClientLoggingHandler()));

        var client = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<AuthorizationClient>());

        var sut = new AuthorizationClient(client);

        var roleId = Guid.NewGuid();
        var parentRoleId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.RoleInRoles
            .AddRangeAsync(new RoleInRole
            {
                RoleId = roleId,
                ParentRoleId = parentRoleId,
                Priority = 100
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.RemoveRoleInRoleAsync(roleId, parentRoleId);
        var actual = await dbContext.RoleInRoles.FindAsync([roleId, parentRoleId]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task 整合測試_取得有指派指定角色的角色Id列表()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging
                    .AddXUnit(testOutputHelper))
                .ConfigureServices(services => services
                    .AddHttpClientLoggingHandler()));

        var client = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<AuthorizationClient>());

        var sut = new AuthorizationClient(client);

        var parentRoleId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.RoleInRoles
            .AddRangeAsync(new RoleInRole
            {
                RoleId = Guid.NewGuid(),
                ParentRoleId = parentRoleId,
                Priority = 100
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.ListChildrenRolesAsync(parentRoleId).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }
}