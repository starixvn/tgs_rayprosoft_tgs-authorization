namespace Tgs.Authorization;

public interface ITgsRepository
{
	ValueTask CreateRoleAsync(Role newRole, CancellationToken cancellationToken = default);

	ValueTask DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Role> GetAccountAllRolesAsync(Guid accountId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<Role> GetSystemAllRolesAsync(string system, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SystemFunction> GetSystemFunctionsAsync(string system, CancellationToken cancellationToken = default);

	IAsyncEnumerable<RoleGrant> GetFunctionGrantRolesAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<AccountGrant> GetFunctionGrantAccountsAsync(Guid functionId, CancellationToken cancellationToken = default);

	ValueTask UpsertAccountFunctionGrantAsync(AccountGrant grant, CancellationToken cancellationToken = default);

	ValueTask UpsertRoleFunctionGrantAsync(RoleGrant grant, CancellationToken cancellationToken = default);

	ValueTask DeleteAccountFunctionGrantAsync(Guid functionId, Guid accountId, CancellationToken cancellationToken = default);

	ValueTask DeleteRoleFunctionGrantAsync(Guid functionId, Guid roleId, CancellationToken cancellationToken = default);

	ValueTask CreateFunctionAsync(SystemFunction function, CancellationToken cancellationToken = default);

	ValueTask RenameFunctionAsync(Guid functionId, string newName, CancellationToken cancellationToken = default);

	ValueTask DeleteFunctionAsync(Guid functionId, CancellationToken cancellationToken = default);
}
