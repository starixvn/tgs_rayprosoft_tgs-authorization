using Valhalla.Authorization;

namespace Tgs.Authorization.Clients.DependencyInjection;

public static class AuthorizationSystemBuilderExtensions
{
	public static AuthorizationSystemBuilder RegisterTgsAuthorizationDataStore(this AuthorizationSystemBuilder builder)
		=> builder.RegisterAuthorizationDataStore<TgsAuthorizationDataStore>();
}