using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tgs.Authorization.Clients;
using Tgs.Authorization.Repositories.Entities;
using Xunit.Abstractions;

namespace Tgs.Authorization.IntegrationTests;

public class GrantTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task 整合測試_設定Account權限()
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

        var accountId = Guid.NewGuid();
        var functionId = Guid.NewGuid();
        var request = new GrantRequest(functionId, Clients.PermissionType.Allow);

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Act
        await sut.AssignAccountGrantAsync(accountId, request);
        var actual = await dbContext.GrantRows.FindAsync([accountId, functionId]);

        // Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public async Task 整合測試_移除Account權限設定()
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
        await sut.RemoveAccountGrantAsync(accountId, functionId);
        var actual = await dbContext.GrantRows.FindAsync([accountId, functionId]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task 整合測試_設定Role權限()
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
        var functionId = Guid.NewGuid();
        var request = new GrantRequest(functionId, Clients.PermissionType.Allow);

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Act
        await sut.AssignRoleGrantAsync(roleId, request);
        var actual = await dbContext.RoleGrantRows.FindAsync([roleId, functionId]);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(request.Type.ToString(), actual.Type);
    }

    [Fact]
    public async Task 整合測試_移除Role權限設定()
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
        var functionId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.RoleGrantRows.AddRangeAsync(
            new RoleGrantRow
            {
                RoleId = roleId,
                FunctionId = functionId,
                Type = nameof(PermissionType.Allow)
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.RemoveRoleGrantAsync(roleId, functionId);
        var actual = await dbContext.RoleGrantRows.FindAsync([roleId, functionId]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task 整合測試_取得Account權限設定列表()
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

        var accountId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.GrantRows.AddRangeAsync(
            new GrantRow
            {
                AccountId = accountId,
                FunctionId = Guid.NewGuid(),
                Type = nameof(PermissionType.Allow)
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.ListAccountGrantsAsync(accountId).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task 整合測試_取得Role權限設定列表()
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

        await dbContext.RoleGrantRows.AddRangeAsync(
            new RoleGrantRow
            {
                RoleId = roleId,
                FunctionId = Guid.NewGuid(),
                Type = nameof(PermissionType.Allow)
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.ListRoleGrantsAsync(roleId).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }
}