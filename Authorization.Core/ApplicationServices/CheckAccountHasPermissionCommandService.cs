using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class CheckAccountHasPermissionCommandService(ITgsRepository tgsRepository)
	: IGetService<CheckAccountHasPermissionCommand, bool>
{
	public async ValueTask<bool> GetAsync(CheckAccountHasPermissionCommand command, CancellationToken cancellationToken = default)
	{
		var grantAccounts = await tgsRepository.GetFunctionGrantAccountsAsync(
			command.FunctionId,
			cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);

		var accountGrant = grantAccounts.FirstOrDefault(grant => grant.AccountId == command.AccountId);

		if (accountGrant is not null)
			return accountGrant.Type == PermissionType.Allow;

		var allAccountPartOfRoles = await tgsRepository.GetAccountAllRolesAsync(
			command.AccountId,
			cancellationToken)
			.Select(role => role.Id)
			.ToHashSetAsync(cancellationToken)
			.ConfigureAwait(false);

		var accountRoleGrant = await tgsRepository.GetFunctionGrantRolesAsync(
			command.FunctionId,
			cancellationToken)
			.Where(grant => allAccountPartOfRoles.Contains(grant.RoleId))
			.OrderByDescending(grant => grant.Priority)
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);

		return accountRoleGrant is null
			? accountGrant is not null
			: accountRoleGrant.Type == PermissionType.Allow;
	}
}
