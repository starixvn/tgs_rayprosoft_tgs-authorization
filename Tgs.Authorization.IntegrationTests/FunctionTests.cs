using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tgs.Authorization.Clients;
using Tgs.Authorization.Repositories.Entities;
using Xunit.Abstractions;

namespace Tgs.Authorization.IntegrationTests;

public class FunctionTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task 整合測試_取得Function列表()
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

        await dbContext.FunctionRows.AddRangeAsync(
            new FunctionRow
            {
                Id = Guid.NewGuid(),
                Name = "func1",
                System = system
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.ListSystemFunctionsAsync(system).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task 整合測試_取得一個Function的Feature設定列表()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging.AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var functionId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.FeatureRows.AddRangeAsync(
            new FeatureRow
            {
                Id = Guid.NewGuid(),
                Name = "feat1",
                FunctionId = functionId,
                Settings = "{}",
                Type = nameof(FeatureType.Http)
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        var actual = await sut.ListSystemFunctionFeaturesAsync(functionId).ToArrayAsync();

        // Assert
        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task 整合測試_建立一個Function設定()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging.AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var data = new CreateFunctionData(
            "func1",
            "Test");

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Act
        var createdId = await sut.CreateFunctionAsync(data);
        var actual = await dbContext.FunctionRows.FindAsync([createdId]);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal((data.Name, data.System), (actual.Name, actual.System));
    }

    [Fact]
    public async Task 整合測試_修改Function名稱()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging.AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var functionId = Guid.NewGuid();
        var newName = "new_name";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.FunctionRows.AddRangeAsync(
            new FunctionRow
            {
                Id = functionId,
                Name = "old_name",
                System = "Test"
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.RenameFunctionAsync(functionId, newName);
        var actual = await dbContext.FunctionRows.FindAsync([functionId]);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(newName, actual.Name);
    }

    [Fact]
    public async Task 整合測試_刪除Function()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging.AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var functionId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.FunctionRows.AddRangeAsync(
            new FunctionRow
            {
                Id = functionId,
                Name = "func1",
                System = "Test"
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.DeleteFunctionAsync(functionId);
        var actual = await dbContext.FunctionRows.FindAsync([functionId]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task 整合測試_為Function新增Feature()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging.AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var functionId = Guid.NewGuid();
        var featureData = new CreateFeatureData(
            "feat1",
            FeatureType.Http,
            JsonDocument.Parse("{}"));

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        // Act
        var createdId = await sut.AddFunctionFeatureAsync(functionId, featureData);
        var actual = await dbContext.FeatureRows.FindAsync([createdId]);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal((featureData.Name, featureData.Type.ToString()), (actual.Name, actual.Type));
    }

    [Fact]
    public async Task 整合測試_移除一個Feature()
    {
        // Arrange
        await using var web = AuthorizationTestHost.Instance
            .WithWebHostBuilder(hostBuilder => hostBuilder
                .ConfigureLogging(logging => logging.AddXUnit(testOutputHelper)));

        var client = web.CreateDefaultClient();

        var sut = new AuthorizationClient(client);

        var featureId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.FeatureRows.AddRangeAsync(
            new FeatureRow
            {
                Id = featureId,
                FunctionId = Guid.NewGuid(),
                Name = "feat1",
                Settings = "{}",
                Type = nameof(FeatureType.Http)
            });

        _ = await dbContext.SaveChangesAsync();

        // Act
        await sut.RemoveFunctionFeatureAsync(featureId);
        var actual = await dbContext.FeatureRows.FindAsync([featureId]);

        // Assert
        Assert.Null(actual);
    }
}