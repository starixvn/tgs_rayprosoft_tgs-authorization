using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class RemoveAccountInRoleCommandService(ITgsRepository tgsRepository)
	: ICommandService<RemoveAccountInRoleCommand>
{
	public ValueTask ExecuteAsync(RemoveAccountInRoleCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.RemoveAccountInRoleAsync(
			command.AccountId,
			command.RoleId,
			cancellationToken);
}