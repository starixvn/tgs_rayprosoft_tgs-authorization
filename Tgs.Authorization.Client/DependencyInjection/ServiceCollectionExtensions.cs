using Tgs.Authorization.Clients;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IHttpClientBuilder AddTgsAuthorizationClient(this IServiceCollection services)
		=> services
			.AddHttpClient<IAuthorizationClient, AuthorizationClient>();
}