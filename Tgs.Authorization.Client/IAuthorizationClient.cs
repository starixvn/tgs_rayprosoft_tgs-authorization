namespace Tgs.Authorization.Clients;

public interface IAuthorizationClient
{
	ValueTask<Guid> AddFunctionFeatureAsync(Guid functionId, CreateFeatureData featureData, CancellationToken cancellationToken = default);

	ValueTask<Guid> CreateFunctionAsync(CreateFunctionData functionData, CancellationToken cancellationToken = default);

	ValueTask DeleteFunctionAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SystemFunctionFeature> ListSystemFunctionFeaturesAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SystemFunction> ListSystemFunctionsAsync(string system, CancellationToken cancellationToken = default);

	ValueTask RemoveFunctionFeatureAsync(Guid featureId, CancellationToken cancellationToken = default);

	ValueTask RenameFunctionAsync(Guid functionId, string newName, CancellationToken cancellationToken = default);

	ValueTask<Guid> CreateRoleAsync(string system, string roleName, CancellationToken cancellationToken = default);

	ValueTask DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Role> GetRolesBySystemAsync(string system, CancellationToken cancellationToken = default);
}