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

	ValueTask AddAccountInRoleAsync(Guid accountId, Guid roleId, int priority, CancellationToken cancellationToken = default);

	ValueTask RemoveAccountInRoleAsync(Guid accountId, Guid roleId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Role> GetRolesBySystemAsync(string system, CancellationToken cancellationToken = default);

	ValueTask AssignAccountGrantAsync(Guid accountId, GrantRequest request, CancellationToken cancellationToken = default);

	ValueTask RemoveAccountGrantAsync(Guid accountId, Guid functionId, CancellationToken cancellationToken = default);

	ValueTask AssignRoleGrantAsync(Guid roleId, RoleGrantRequest request, CancellationToken cancellationToken = default);

	ValueTask RemoveRoleGrantAsync(Guid roleId, Guid functionId, CancellationToken cancellationToken = default);

	ValueTask<bool> AuthorizeAsync(Guid accountId, Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> ListRoleAccountsAsync(Guid roleId, CancellationToken cancellationToken = default);

	ValueTask AddRoleInRoleAsync(Guid roleId, Guid parentRoleId, int priority, CancellationToken cancellationToken = default);

	ValueTask RemoveRoleInRoleAsync(Guid roleId, Guid parentRoleId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> ListChildrenRolesAsync(Guid parentRoleId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<AccountGrant> ListAccountGrantsAsync(Guid accountId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<RoleGrant> ListRoleGrantsAsync(Guid roleId, CancellationToken cancellationToken = default);
}