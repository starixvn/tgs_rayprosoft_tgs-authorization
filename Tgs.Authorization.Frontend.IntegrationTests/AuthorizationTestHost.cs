using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using Tgs.Authorization.Repositories.Entities;

namespace Tgs.Authorization.Frontend.IntegrationTests;

internal static class AuthorizationTestHost
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

	internal static IHostBuilder UseTestChannelServer(this IHostBuilder hostBuilder, string channelName)
	{
		var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

		return isWindows
			? hostBuilder
				.ConfigureWebHost(webHostBuilder => webHostBuilder
					.UseKestrel(options => options.ListenNamedPipe(channelName)))
			: hostBuilder
				.ConfigureWebHost(webHostBuilder => webHostBuilder
					.UseKestrel(options => options.ListenUnixSocket(
						$"/tmp/{channelName}.sock")));
	}

	public static IHost BuildTestHost(
		string channelName,
		Action<IHostBuilder>? configureHostBuilder = null)
	{
		var deferredHostBuilder = new DeferredHostBuilder();
		deferredHostBuilder.UseEnvironment(Environments.Development);

		// There's no helper for UseApplicationName, but we need to 
		// set the application name to the target entry point 
		// assembly name.
		deferredHostBuilder.ConfigureHostConfiguration(config => config
			.AddInMemoryCollection(new Dictionary<string, string?>
			{
				{ HostDefaults.ApplicationKey, typeof(Program).Assembly.GetName()?.Name ?? string.Empty }
			}));

		var factory = HostFactoryResolver.ResolveHostFactory(
			typeof(Program).Assembly,
			stopApplication: false,
			configureHostBuilder: deferredHostBuilder.ConfigureHostBuilder,
			entrypointCompleted: deferredHostBuilder.EntryPointCompleted);

		deferredHostBuilder.SetHostFactory(factory!);

		_ = deferredHostBuilder
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
				.PostConfigure<OtlpExporterOptions>(options => options.TimeoutMilliseconds = 0))
			.UseTestChannelServer(channelName);

		configureHostBuilder?.Invoke(deferredHostBuilder);

		return deferredHostBuilder.Build();
	}
}