using System.Net.Http.Json;

namespace Tgs.Authorization.Clients;

internal class AuthorizationClient(HttpClient httpClient) : IAuthorizationClient
{
	public ValueTask<Guid> AddFunctionFeatureAsync(
		Guid functionId,
		CreateFeatureData featureData,
		CancellationToken cancellationToken = default)
		=> httpClient.PutAsJsonAsync(
			$"/function/feature/{functionId}",
			featureData,
			ClientUtility.SerializerOptions,
			cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<Guid>(ClientUtility.SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.Take(1)
			.SingleAsync(cancellationToken);

	public ValueTask<Guid> CreateFunctionAsync(CreateFunctionData functionData, CancellationToken cancellationToken = default)
		=> httpClient.PutAsJsonAsync(
			"/function",
			functionData,
			ClientUtility.SerializerOptions,
			cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<Guid>(ClientUtility.SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.Take(1)
			.SingleAsync(cancellationToken);

	public async ValueTask DeleteFunctionAsync(Guid functionId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync(
			$"/function/{functionId}",
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public IAsyncEnumerable<SystemFunctionFeature> ListSystemFunctionFeaturesAsync(
		Guid functionId,
		CancellationToken cancellationToken = default)
		=> httpClient.GetAsync($"/function/feature/{functionId}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsAsyncEnumerable<SystemFunctionFeature>(ClientUtility.SerializerOptions, cancellationToken))
			.Where(feat => feat is not null)
			.Select(feat => feat!);

	public IAsyncEnumerable<SystemFunction> ListSystemFunctionsAsync(string system, CancellationToken cancellationToken = default)
		=> httpClient.GetAsync($"/function/{system}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsAsyncEnumerable<SystemFunction>(ClientUtility.SerializerOptions, cancellationToken))
			.Where(sysFunc => sysFunc is not null)
			.Select(sysFunc => sysFunc!);

	public async ValueTask RemoveFunctionFeatureAsync(Guid featureId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync(
			$"/function/feature/{featureId}",
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask RenameFunctionAsync(Guid functionId, string newName, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PatchAsJsonAsync(
			$"/function/{functionId}",
			newName,
			ClientUtility.SerializerOptions,
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public ValueTask<Guid> CreateRoleAsync(string system, string roleName, CancellationToken cancellationToken = default)
		=> httpClient.PutAsJsonAsync(
			$"/role/{system}",
			new
			{
				Name = roleName
			},
			ClientUtility.SerializerOptions,
			cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<Guid>(ClientUtility.SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.Take(1)
			.SingleAsync(cancellationToken);

	public async ValueTask DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync(
			$"/role/{roleId}",
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public IAsyncEnumerable<Role> GetRolesBySystemAsync(string system, CancellationToken cancellationToken = default)
		=> httpClient.GetAsync($"/role/{system}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsAsyncEnumerable<Role>(ClientUtility.SerializerOptions, cancellationToken))
			.Where(role => role is not null)
			.Select(role => role!);

	public async ValueTask AssignAccountGrantAsync(Guid accountId, GrantRequest request, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PutAsJsonAsync(
			$"/grant/account/{accountId}",
			request,
			ClientUtility.SerializerOptions,
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask RemoveAccountGrantAsync(Guid accountId, Guid functionId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync(
			$"/grant/account/{accountId}/{functionId}",
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask AssignRoleGrantAsync(Guid roleId, RoleGrantRequest request, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.PutAsJsonAsync(
			$"/grant/role/{roleId}",
			request,
			ClientUtility.SerializerOptions,
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask RemoveRoleGrantAsync(Guid roleId, Guid functionId, CancellationToken cancellationToken = default)
	{
		var response = await httpClient.DeleteAsync(
			$"/grant/role/{roleId}/{functionId}",
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}
}