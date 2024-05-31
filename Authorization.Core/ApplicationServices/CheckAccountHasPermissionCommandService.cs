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
			true,
			cancellationToken)
			.OrderBy(role => role.Level)
			.ThenByDescending(role => role.Priority)
			.ToArrayAsync(cancellationToken)
			.ConfigureAwait(false);

		var accountRoleGrants = await tgsRepository.GetFunctionGrantRolesAsync(
			command.FunctionId,
			cancellationToken)
			.ToArrayAsync(cancellationToken)
			.ConfigureAwait(false);

		foreach (var role in allAccountPartOfRoles)
		{
			var grant = accountRoleGrants.FirstOrDefault(g => g.RoleId == role.Id);

			if (grant is not null)
				return grant.Type == PermissionType.Allow;
		}

		return false;
	}
}