using System.Collections.Concurrent;
using Valhalla.Authorization;
using Valhalla.Authorization.AspNetCore;

namespace Tgs.Authorization.Clients;

internal class TgsAuthorizationDataStore(
	IAuthorizationClient authorizationClient,
	IServiceProvider serviceProvider)
	: IAuthorizationDataStore
{
	private readonly ConcurrentDictionary<string, IAuthorizationFunction[]> _functionsCache = new();

	public IAsyncEnumerable<IAuthorizationFunction> GetFunctionsAsync(string system, CancellationToken cancellationToken = default)
		=> _functionsCache.GetOrAdd(
			system,
			_ => authorizationClient.ListSystemFunctionsAsync(system, cancellationToken)
				.Select(sysFunc => new HttpFunction(
					sysFunc.Id,
					sysFunc.Name,
					false,
					authorizationClient.ListSystemFunctionFeaturesAsync(sysFunc.Id, cancellationToken)
						.Select(sysFeat => sysFeat.GetSettings<HttpFeatureSettings>())
						.Where(settings => settings is not null)
						.Select(settings => new HttpCompositeFeature(
							new HttpMethodFeature(settings!.HttpMethod.ToArray()),
							new HttpPathFeature(settings.PathTemplate, serviceProvider)))
						.ToEnumerable()
						.ToArray()))
				.ToEnumerable()
				.ToArray())
			.ToAsyncEnumerable();

	public ValueTask<IAuthorizationFunction?> FindFunctionAsync(string system, string functionName, CancellationToken cancellationToken = default)
		=> GetFunctionsAsync(system, cancellationToken)
			.FirstOrDefaultAsync(
				func => func.Name == functionName,
				cancellationToken);

	public async ValueTask<bool> CheckHasPermissionAsync(
		string system,
		IAuthorizationFunction function,
		IEnumerable<IAuthorizationIdentity> identities,
		CancellationToken cancellationToken = default)
	{
		var account = identities.OfType<TgsAccount>().FirstOrDefault();

		return account is not null
			&& await authorizationClient.AuthorizeAsync(
			account.Id,
			function.Id,
			cancellationToken).ConfigureAwait(false);
	}
}
