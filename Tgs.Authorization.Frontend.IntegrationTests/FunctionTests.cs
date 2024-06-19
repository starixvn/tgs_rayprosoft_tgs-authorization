using System.Runtime.InteropServices;
using ExpectedObjects;
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
}
