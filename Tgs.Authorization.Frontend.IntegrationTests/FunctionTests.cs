using System.Runtime.InteropServices;
using ExpectedObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tgs.Authorization.Repositories.Entities;
using Tgs.Authorization.ViewModels;
using Xunit.Abstractions;

namespace Tgs.Authorization.Frontend.IntegrationTests;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "<暫止>")]
public class FunctionTests(NodeJsFixture nodeJsFixture, ITestOutputHelper testOutputHelper)
{
	[Fact]
	public async Task 前端整合測試_取得系統功能定義列表()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging((_, logging) => logging
					.ClearProviders()
					.AddXUnit(testOutputHelper)));

		var system = "Test";

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		var rows = new[]
		{
			new FunctionRow
			{
				Id = Guid.NewGuid(),
				Name = "Func1",
				System = system
			}
		};

		await dbContext.FunctionRows.AddRangeAsync(rows);

		_ = await dbContext.SaveChangesAsync();

		var expected = rows.Select(
				row => new SystemFunctionViewModel
				{
					Id = row.Id,
					Name = row.Name,
					System = row.System
				})
			.ToArray()
			.ToExpectedObject();

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
		    const utils = require('./utils.cjs');
		    const toArray = require('rxjs').toArray;
		    const firstValueFrom = require('rxjs').firstValueFrom;

		    module.exports = async (channelName, isWindows, system) => {
		    let m = await utils();
		    let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

		    return await firstValueFrom(client.getFunctionsBySystem(system)
		        .pipe(toArray()));
		    };
		    """;

		// Act
		var actual = await nodejs.InvokeFromStringAsync<SystemFunctionViewModel[]>(
			nodeModule,
			args: [channelName, isWindows, system]);

		// Assert
		expected.ShouldMatch(actual);

		_ = web.StopAsync();
	}

	[Fact]
	public async Task 前端整合測試_建立一個Function設定()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging((_, logging) => logging
					.ClearProviders()
					.AddXUnit(testOutputHelper)));

		var functionName = "func1";
		var system = "Test";

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const map = require('rxjs').map;
			const firstValueFrom = require('rxjs').firstValueFrom;

			module.exports = async (channelName, isWindows, system, name) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				return await firstValueFrom(client.createFunction(system, name)
					.pipe(
						map(id => JSON.stringify(id))
					));
			};
			""";

		// Act
		var createdId = await nodejs.InvokeFromStringAsync<Guid>(
			nodeModule,
			args: [channelName, isWindows, system, functionName]);
		var actual = await dbContext.FunctionRows.FindAsync([createdId]);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal((functionName, system), (actual.Name, actual.System));
	}

	[Fact]
	public async Task 前端整合測試_修改Function名稱()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging((_, logging) => logging
					.ClearProviders()
					.AddXUnit(testOutputHelper)));

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

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const map = require('rxjs').map;
			const firstValueFrom = require('rxjs').firstValueFrom;

			module.exports = async (channelName, isWindows, functionId, newName) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				await firstValueFrom(client.renameFunction(functionId, newName));
			};
			""";

		// Act
		await nodejs.InvokeFromStringAsync(
			nodeModule,
			args: [channelName, isWindows, functionId, newName]);
		var actual = await dbContext.FunctionRows.FindAsync([functionId]);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(newName, actual.Name);
	}

	[Fact]
	public async Task 前端整合測試_刪除Function()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging((_, logging) => logging
					.ClearProviders()
					.AddXUnit(testOutputHelper)));

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

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const map = require('rxjs').map;
			const firstValueFrom = require('rxjs').firstValueFrom;

			module.exports = async (channelName, isWindows, functionId) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				await firstValueFrom(client.deleteFunction(functionId));
			};
			""";

		// Act
		await nodejs.InvokeFromStringAsync(
			nodeModule,
			args: [channelName, isWindows, functionId]);
		var actual = await dbContext.FunctionRows.FindAsync([functionId]);

		// Assert
		Assert.Null(actual);
	}

	[Fact]
	public async Task 前端整合測試_取得一個Function的Feature設定列表()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging((_, logging) => logging
					.ClearProviders()
					.AddXUnit(testOutputHelper)));

		var functionId = Guid.NewGuid();

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		await dbContext.FeatureRows.AddRangeAsync(
			new FeatureRow
			{
				Id = Guid.NewGuid(),
				Name = "feat1",
				FunctionId = functionId,
				Settings = "{}",
				Type = "Http"
			});

		_ = await dbContext.SaveChangesAsync();

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const toArray = require('rxjs').toArray;
			const firstValueFrom = require('rxjs').firstValueFrom;

			module.exports = async (channelName, isWindows, functionId) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				return await firstValueFrom(client.getFunctionFeatures(functionId)
					.pipe(
						toArray()
					));
			};
			""";

		// Act
		var actual = await nodejs.InvokeFromStringAsync<SystemFunctionFeatureViewModel[]>(
			nodeModule,
			args: [channelName, isWindows, functionId]);

		// Assert
		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
	}
}
