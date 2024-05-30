using System.Text.Json;

namespace Tgs.Authorization;

public interface ITgsRepository
{
	ValueTask CreateFunctionAsync(SystemFunction function, CancellationToken cancellationToken = default);

	ValueTask CreateRoleAsync(Role newRole, CancellationToken cancellationToken = default);

	ValueTask DeleteAccountFunctionGrantAsync(Guid functionId, Guid accountId, CancellationToken cancellationToken = default);

	ValueTask DeleteFunctionAsync(Guid functionId, CancellationToken cancellationToken = default);

	ValueTask DeleteFunctionFeatureAsync(Guid featureId, CancellationToken cancellationToken = default);

	ValueTask DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

	ValueTask DeleteRoleFunctionGrantAsync(Guid roleId, Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Role> GetAccountAllRolesAsync(Guid accountId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<AccountGrant> GetFunctionGrantAccountsAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<RoleGrant> GetFunctionGrantRolesAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Role> GetSystemAllRolesAsync(string system, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SystemFunctionFeature<JsonDocument>> GetSystemFunctionFeaturesAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SystemFunction> GetSystemFunctionsAsync(string system, CancellationToken cancellationToken = default);

	ValueTask InsertFunctionFeatureAsync(Guid functionId, SystemFunctionFeature<string> feature, CancellationToken cancellationToken = default);

	ValueTask RenameFunctionAsync(Guid functionId, string newName, CancellationToken cancellationToken = default);

	ValueTask UpsertAccountFunctionGrantAsync(AccountGrant grant, CancellationToken cancellationToken = default);

	ValueTask UpsertRoleFunctionGrantAsync(RoleGrant grant, CancellationToken cancellationToken = default);

	ValueTask UpsertAccountInRoleAsync(Guid accountId, Guid roleId, int priority, CancellationToken cancellationToken = default);

	ValueTask RemoveAccountInRoleAsync(Guid accountId, Guid roleId, CancellationToken cancellationToken = default);

	ValueTask UpsertRoleInRoleAsync(Guid roleId, Guid parentRoleId, int priority, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> GetRoleAccountsAsync(Guid roleId, CancellationToken cancellationToken = default);

	ValueTask RemoveRoleInRoleAsync(Guid roleId, Guid parentRoleId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Guid> GetChildrenRolesAsync(Guid parentRoleId, CancellationToken cancellationToken = default);
}