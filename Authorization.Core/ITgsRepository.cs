namespace Tgs.Authorization;

public interface ITgsRepository
{
	IAsyncEnumerable<Role> GetAccountAllRolesAsync(Guid accountId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SystemFunction> GetSystemFunctionsAsync(string system, CancellationToken cancellationToken = default);

	IAsyncEnumerable<RoleGrant> GetFunctionGrantRolesAsync(Guid functionId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<AccountGrant> GetFunctionGrantAccountsAsync(Guid functionId, CancellationToken cancellationToken = default);
}
