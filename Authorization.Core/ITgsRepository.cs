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
}
