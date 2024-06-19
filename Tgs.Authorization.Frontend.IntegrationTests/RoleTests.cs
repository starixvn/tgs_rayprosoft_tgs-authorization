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
public class RoleTests(NodeJsFixture nodeJsFixture, ITestOutputHelper testOutputHelper)
{
	[Fact]
	public async Task 前端整合測試_取得系統角色列表()
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
			new RoleRow
			{
				Id = Guid.NewGuid(),
				Name = "Group1",
				System = system,
				Priority = 1
			}
		};

		await dbContext.RoleRows.AddRangeAsync(rows);

		_ = await dbContext.SaveChangesAsync();

		var expected = rows.Select(
			row => new RoleViewModel
			{
				Id = row.Id,
				Name = row.Name,
				Priority = row.Priority
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

				return await firstValueFrom(client.getRolesBySystem(system)
					.pipe(toArray()));
			};
			""";

		// Act
		var actual = await nodejs.InvokeFromStringAsync<RoleViewModel[]>(
			nodeModule,
			args: [channelName, isWindows, system]);

		// Assert
		expected.ShouldMatch(actual);

		_ = web.StopAsync();
	}

	[Fact]
	public async Task 前端整合測試_建立角色()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging(logging => logging
					.AddXUnit(testOutputHelper)));

		var system = "Test";
		var roleName = "Group1";

		var request = new CreateRoleViewModel
		{
			Name = roleName
		};

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const firstValueFrom = require('rxjs').firstValueFrom;
			const map = require('rxjs').map;

			module.exports = async (channelName, isWindows, system, request) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				return await firstValueFrom(client.createRole(system, request)
					.pipe(
						map(id => JSON.stringify(id))
					));
			};
			""";

		// Acta
		var createdId = await nodejs.InvokeFromStringAsync<Guid>(
			nodeModule,
			args: [channelName, isWindows, system, request]);
		var actual = await dbContext.RoleRows.FindAsync([createdId]);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(system, actual.System);
		Assert.Equal(roleName, actual.Name);

		_ = web.StopAsync();
	}

	[Fact]
	public async Task 前端整合測試_刪除角色()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging(logging => logging
					.AddXUnit(testOutputHelper)));

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

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const firstValueFrom = require('rxjs').firstValueFrom;
			const map = require('rxjs').map;

			module.exports = async (channelName, isWindows, roleId) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				await firstValueFrom(client.deleteRole(roleId));
			};
			""";

		// Act
		await nodejs.InvokeFromStringAsync(
			nodeModule,
			args: [channelName, isWindows, roleId]);
		var actual = await dbContext.RoleRows.FindAsync([roleId]);

		// Assert
		Assert.Null(actual);

		_ = web.StopAsync();
	}

	[Fact]
	public async Task 前端整合測試_設定角色優先權()
	{
		// Arrange
		var channelName = Guid.NewGuid().ToString();

		using var web = AuthorizationTestHost.BuildTestHost(
			channelName,
			hostBuilder => hostBuilder
				.ConfigureLogging(logging => logging
					.AddXUnit(testOutputHelper)));

		var roleId = Guid.NewGuid();

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		await dbContext.RoleRows.AddRangeAsync(
			new RoleRow
			{
				Id = roleId,
				Name = "Test1",
				System = "Test",
				Priority = 100
			});

		_ = await dbContext.SaveChangesAsync();

		var nodejs = nodeJsFixture.BuildNodeJsService();

		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		var nodeModule = """
			const utils = require('./utils.cjs');
			const firstValueFrom = require('rxjs').firstValueFrom;
			const map = require('rxjs').map;

			module.exports = async (channelName, isWindows, roleId, priority) => {
				let m = await utils();
				let client = new m.AuthorizeClient(m.buildHttpClient(channelName, isWindows));

				await firstValueFrom(client.setRolePriority(roleId, priority));
			};
			""";

		// Act
		await nodejs.InvokeFromStringAsync(
			nodeModule,
			args: [channelName, isWindows, roleId, 200]);
		var actual = await dbContext.RoleRows.FindAsync([roleId]);

		// Assert
		Assert.Equal(200, actual!.Priority);

		_ = web.StopAsync();
	}
}
